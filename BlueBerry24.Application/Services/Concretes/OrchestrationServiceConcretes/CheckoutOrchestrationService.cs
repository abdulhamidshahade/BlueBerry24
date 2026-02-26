using BlueBerry24.Application.Dtos.OrderDtos;
using BlueBerry24.Application.Services.Interfaces.CouponServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.InventoryServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.OrderServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.OrchestrationServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces;
using BlueBerry24.Domain.Constants;
using BlueBerry24.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry24.Application.Services.Concretes.OrchestrationServiceConcretes
{
    public class CheckoutOrchestrationService : ICheckoutOrchestrationService
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly IInventoryService _inventoryService;
        private readonly IUserCouponService _userCouponService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CheckoutOrchestrationService> _logger;

        public CheckoutOrchestrationService(
            ICartService cartService,
            IOrderService orderService,
            IInventoryService inventoryService,
            IUserCouponService userCouponService,
            IUnitOfWork unitOfWork,
            ILogger<CheckoutOrchestrationService> logger)
        {
            _cartService = cartService;
            _orderService = orderService;
            _inventoryService = inventoryService;
            _userCouponService = userCouponService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<CheckoutResult> ProcessCheckoutAsync(int cartId, CreateOrderDto orderDto, int? userId)
        {
            var result = new CheckoutResult();

            try
            {
                _logger.LogInformation("Starting checkout process for cart {CartId}", cartId);

                var cart = await _cartService.GetCartByIdAsync(cartId, CartStatus.Active);
                if (cart == null)
                {
                    result.ErrorMessage = "Cart not found or inactive";
                    return result;
                }

                if (cart.CartItems == null || cart.CartItems.Count == 0)
                {
                    result.ErrorMessage = "Cart is empty";
                    return result;
                }

                foreach (var item in cart.CartItems)
                {
                    var isInStock = await _inventoryService.IsInStockAsync(item.ProductId, item.Quantity);
                    if (!isInStock)
                    {
                        result.ErrorMessage = $"Insufficient stock for product ID {item.ProductId}";
                        return result;
                    }
                }

                var strategy = _unitOfWork.BeginTransactionAsyncStrategy();
                var transactionResult = await strategy.ExecuteAsync(async () =>
                {
                    await _unitOfWork.BeginTransactionAsync();

                    try
                    {
                        _logger.LogDebug("Creating order from cart {CartId}", cartId);
                        var order = await _orderService.CreateOrderFromCartAsync(cartId, orderDto);
                        if (order == null)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            result.ErrorMessage = "Failed to create order";
                            return false;
                        }

                        result.Order = order;

                        _logger.LogDebug("Deducting inventory for order {OrderId}", order.Id);
                        foreach (var item in cart.CartItems)
                        {
                            var deductionConfirmed = await _inventoryService.ConfirmStockDeductionAsync(
                                item.ProductId,
                                item.Quantity,
                                order.Id,
                                "Order");

                            if (!deductionConfirmed)
                            {
                                await _unitOfWork.RollbackTransactionAsync();
                                result.ErrorMessage = $"Failed to deduct inventory for product {item.ProductId}";
                                return false;
                            }
                        }

                        _logger.LogDebug("Marking coupons as used for order {OrderId}", order.Id);
                        if (userId.HasValue && cart.CartCoupons != null && cart.CartCoupons.Any())
                        {
                            foreach (var cartCoupon in cart.CartCoupons)
                            {
                                var couponMarked = await _userCouponService.MarkCouponAsUsedAsync(
                                    userId.Value,
                                    cartCoupon.CouponId,
                                    order.Id);

                                if (!couponMarked)
                                {
                                    result.Warnings.Add($"Failed to mark coupon {cartCoupon.CouponId} as used");
                                }
                            }
                        }

                        _logger.LogDebug("Clearing cart {CartId}", cartId);
                        var cartCleared = await _cartService.ClearCartAsync(cartId, userId, null);
                        if (!cartCleared)
                        {
                            result.Warnings.Add("Failed to clear cart after checkout");
                        }

                        var committed = await _unitOfWork.CommitTransactionAsync();
                        if (!committed)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            result.ErrorMessage = "Failed to commit checkout transaction";
                            return false;
                        }

                        result.IsSuccess = true;
                        _logger.LogInformation("Successfully completed checkout for cart {CartId}, order {OrderId}",
                            cartId, order.Id);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error during checkout transaction for cart {CartId}", cartId);
                        await _unitOfWork.RollbackTransactionAsync();
                        result.ErrorMessage = $"Checkout transaction failed: {ex.Message}";
                        return false;
                    }
                });

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during checkout for cart {CartId}", cartId);
                result.ErrorMessage = $"Unexpected error: {ex.Message}";
                return result;
            }
        }
    }
}
