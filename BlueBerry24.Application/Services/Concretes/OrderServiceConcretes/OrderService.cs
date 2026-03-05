using AutoMapper;
using BlueBerry24.Application.Dtos.OrderDtos;
using BlueBerry24.Application.Services.Interfaces.InventoryServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.OrderServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.OrchestrationServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces;
using BlueBerry24.Domain.Constants;
using BlueBerry24.Domain.Entities.OrderEntities;
using BlueBerry24.Domain.Repositories.OrderInterfaces;

namespace BlueBerry24.Application.Services.Concretes.OrderServiceConcretes
{
    public class OrderService : IOrderService
    {
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly IInventoryService _inventoryService;
        private readonly IOrderCancellationService _orderCancellationService;
        private readonly IRefundOrchestrationService _refundOrchestrationService;

        public OrderService(ICartService cartService,
                            IMapper mapper,
                            IOrderRepository orderRepository,
                            IInventoryService inventoryService,
                            IOrderCancellationService orderCancellationService,
                            IRefundOrchestrationService refundOrchestrationService)
        {
            _cartService = cartService;
            _mapper = mapper;
            _orderRepository = orderRepository;
            _inventoryService = inventoryService;
            _orderCancellationService = orderCancellationService;
            _refundOrchestrationService = refundOrchestrationService;
        }


        public async Task<OrderTotal> CalculateOrderTotalsAsync(int userId)
        {
            var cart = await _cartService.GetCartByUserIdAsync(userId, CartStatus.Converted);

            if (cart == null)
            {
                return null;
            }

            decimal subTotal = cart.CartItems.Sum(item => item.UnitPrice * item.Quantity);
            decimal discountTotal = cart.CartCoupons.Sum(coupon => coupon.DiscountAmount);
            decimal taxAmount = (subTotal - discountTotal) * 0.1m;
            decimal shippingAmount = subTotal > 100 ? 0 : 10;

            return new OrderTotal
            {
                SubTotal = subTotal,
                DiscountTotal = discountTotal,
                TaxAmount = taxAmount,
                ShippingAmount = shippingAmount,
                Total = subTotal - discountTotal + taxAmount + shippingAmount
            };
        }

        public async Task<bool> CancelOrderAsync(int orderId, string reason)
        {
            var result = await _orderCancellationService.CancelOrderAsync(orderId, reason);
            return result.IsSuccess;
        }

        public async Task<Order?> CreateOrderFromCartAsync(int cartId, CreateOrderDto orderDto)
        {
            var cart = await _cartService.GetCartByIdAsync(cartId, CartStatus.Active);

            if (cart == null)
            {
                return null;
            }

            if (cart.CartItems == null || !cart.CartItems.Any())
            {
                return null;
            }

            decimal subTotal = cart.CartItems.Sum(item => item.UnitPrice * item.Quantity);
            decimal discountTotal = cart.CartCoupons?.Sum(coupon => coupon.DiscountAmount) ?? 0;
            decimal taxAmount = (subTotal - discountTotal) * 0.1m;
            decimal shippingAmount = subTotal > 100 ? 0 : 10;
            decimal total = subTotal - discountTotal + taxAmount + shippingAmount;

            var referenceNumber = await GenerateUniqueReferenceNumberAsync();

            var order = new Order
            {
                UserId = orderDto.UserId,
                CartId = cartId,
                ReferenceNumber = referenceNumber,
                Status = OrderStatus.Pending,
                SubTotal = subTotal,
                DiscountTotal = discountTotal,
                TaxAmount = taxAmount,
                ShippingAmount = shippingAmount,
                Total = total,
                CustomerEmail = orderDto.CustomerEmail,
                CustomerPhone = orderDto.CustomerPhone,
                ShippingName = orderDto.ShippingName,
                ShippingAddress1 = orderDto.ShippingAddressLine1,
                ShippingAddress2 = orderDto.ShippingAddressLine2,
                ShippingCity = orderDto.ShippingCity,
                ShippingState = orderDto.ShippingState,
                ShippingPostalCode = orderDto.ShippingPostalCode,
                ShippingCountry = orderDto.ShippingCountry,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _orderRepository.CreateOrderAsync(order);

            if (order == null)
            {
                return null;
            }

            foreach (var cartItem in cart.CartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    ProductName = cartItem.Product?.Name ?? "Unknown Product",
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.UnitPrice,
                    TotalPrice = cartItem.UnitPrice * cartItem.Quantity,
                    DiscountAmount = 0
                };

                await _orderRepository.CreateOrderItemAsync(orderItem);
            }

            var cartStatusUpdated = await _cartService.UpdateCartStatusAsync(cartId, CartStatus.PendingPayment);
            if (!cartStatusUpdated)
            {
                throw new InvalidOperationException($"Failed to update cart {cartId} status to PendingPayment. Order creation aborted.");
            }

            return order;
        }

        public async Task<bool> RefundOrderAsync(int orderId, string reason)
        {
            var result = await _refundOrchestrationService.ProcessRefundAsync(orderId, reason);
            return result.IsSuccess;
        }

        public async Task<bool> UpdateOrderStatusAsync(Order order, OrderStatus newStatus)
        {
            if((int)newStatus < 0 || (int)newStatus > 6)
            {
                return false;
            }

            if (order == null)
            {
                return false;
            }

            var oldStatus = order.Status;
            order.Status = newStatus;
            order.UpdatedAt = DateTime.UtcNow;

            switch (newStatus)
            {
                case OrderStatus.Completed:
                    order.CompletedAt = DateTime.UtcNow;
                    break;
                case OrderStatus.Cancelled:
                    order.CancalledAt = DateTime.UtcNow;
                    break;
                case OrderStatus.Processing:
                    if (oldStatus == OrderStatus.Pending)
                    {
                        //TODO Mark as processing - could trigger inventory allocation if needed
                    }
                    break;
                case OrderStatus.Shipped:
                    if (oldStatus == OrderStatus.Processing)
                    {
                        //TODO Order has been shipped - could trigger shipping notifications
                    }
                    break;
                case OrderStatus.Delivered:
                    if (oldStatus == OrderStatus.Shipped)
                    {
                        //TODO Order has been delivered - could trigger completion workflows
                    }
                    break;
            }

            return await _orderRepository.UpdateOrderStatusAsync(order.Id, order.Status);
        }

        public async Task<string> GenerateUniqueReferenceNumberAsync()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var randomComponent = new Random().Next(1000, 9999);
            var referenceNumber = $"ORD-{timestamp}-{randomComponent}";

            var existingOrder = await _orderRepository.GetOrderByReferenceNumberAsync(referenceNumber);

            if (existingOrder != null)
            {
                var additionalRandom = new Random().Next(100, 999);
                referenceNumber = $"ORD-{timestamp}-{randomComponent}-{additionalRandom}";
            }

            return referenceNumber;
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<Order?> GetOrderByReferenceNumberAsync(string referenceNumber)
        {
            var order = await _orderRepository.GetOrderByReferenceNumberAsync(referenceNumber);
            return order;
        }

        public async Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status, int page = 1, int pageSize = 10)
        {
            var orders = await _orderRepository.GetOrdersByStatusAsync(status, page, pageSize);
            return orders;
        }

        public async Task<List<OrderDto>> GetUserOrdersAsync(int userId, int page = 1, int pageSize = 10)
        {
            var orders = await _orderRepository.GetUserOrdersAsync(userId, page, pageSize);

            var mappedOrder = _mapper.Map<List<OrderDto>>(orders);
            return mappedOrder;
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync(int page = 1, int pageSize = 50)
        {
            var orders = await _orderRepository.GetAllOrdersAsync(page, pageSize);

            var mappedOrders = _mapper.Map<List<OrderDto>>(orders);
            return mappedOrders;
        }


        public async Task<bool> ProcessOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            if (order == null)
            {
                return false;
            }

            if (order.Status != OrderStatus.Pending)
            {
                return false;
            }

            order.Status = OrderStatus.Processing;
            order.UpdatedAt = DateTime.UtcNow;

            var orderStatusUpdated = await UpdateOrderStatusAsync(order, order.Status);

            return orderStatusUpdated;
        }

        public Task<bool> UpdateOrderPaymentStatusAsync(Order order, PaymentStatus paymentStatus)
        {
            return _orderRepository.UpdateOrderPaymentStatusAsync(order.Id, paymentStatus);
        }

        public async Task<Order?> GetOrderByCartIdAsync(int cartId)
        {
            var order = await _orderRepository.GetOrderByCartIdAsync(cartId);
            return order;
        }

        public async Task<bool> SyncOrderWithCartAsync(int orderId, int cartId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null || order.Status != OrderStatus.Pending)
            {
                return false;
            }

            var cart = await _cartService.GetCartByIdAsync(cartId, CartStatus.Active);
            if (cart == null)
            {
                cart = await _cartService.GetCartByIdAsync(cartId, CartStatus.PendingPayment);
            }

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                return false;
            }

            decimal subTotal = cart.CartItems.Sum(item => item.UnitPrice * item.Quantity);
            decimal discountTotal = cart.CartCoupons?.Sum(coupon => coupon.DiscountAmount) ?? 0;
            decimal taxAmount = (subTotal - discountTotal) * 0.1m;
            decimal shippingAmount = subTotal > 100 ? 0 : 10;
            decimal total = subTotal - discountTotal + taxAmount + shippingAmount;

            order.SubTotal = subTotal;
            order.DiscountTotal = discountTotal;
            order.TaxAmount = taxAmount;
            order.ShippingAmount = shippingAmount;
            order.Total = total;
            order.UpdatedAt = DateTime.UtcNow;

            await _orderRepository.DeleteOrderItemsAsync(orderId);

            foreach (var cartItem in cart.CartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    ProductName = cartItem.Product?.Name ?? "Unknown Product",
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.UnitPrice,
                    TotalPrice = cartItem.UnitPrice * cartItem.Quantity,
                    DiscountAmount = 0
                };

                await _orderRepository.CreateOrderItemAsync(orderItem);
            }

            return await _orderRepository.UpdateOrderAsync(order);
        }
    }
}
