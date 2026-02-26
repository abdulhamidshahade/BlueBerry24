using BlueBerry24.Application.Services.Interfaces.CouponServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.InventoryServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.OrderServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.OrchestrationServiceInterfaces;
using BlueBerry24.Domain.Constants;
using BlueBerry24.Domain.Repositories;
using BlueBerry24.Domain.Repositories.OrderInterfaces;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry24.Application.Services.Concretes.OrchestrationServiceConcretes
{
    public class OrderCancellationService : IOrderCancellationService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IInventoryService _inventoryService;
        private readonly IUserCouponService _userCouponService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OrderCancellationService> _logger;

        public OrderCancellationService(
            IOrderRepository orderRepository,
            IInventoryService inventoryService,
            IUserCouponService userCouponService,
            IUnitOfWork unitOfWork,
            ILogger<OrderCancellationService> logger)
        {
            _orderRepository = orderRepository;
            _inventoryService = inventoryService;
            _userCouponService = userCouponService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<CancellationResult> CancelOrderAsync(int orderId, string reason, int? performedByUserId = null)
        {
            var result = new CancellationResult();

            try
            {
                _logger.LogInformation("Starting order cancellation for order {OrderId}", orderId);

                var order = await _orderRepository.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    result.ErrorMessage = "Order not found";
                    return result;
                }

                if (order.Status != OrderStatus.Pending && order.Status != OrderStatus.Processing)
                {
                    result.ErrorMessage = $"Cannot cancel order with status {order.Status}. Only Pending or Processing orders can be cancelled.";
                    return result;
                }

                var strategy = _unitOfWork.BeginTransactionAsyncStrategy();
                var transactionResult = await strategy.ExecuteAsync(async () =>
                {
                    await _unitOfWork.BeginTransactionAsync();

                    try
                    {
                        _logger.LogDebug("Restoring inventory for cancelled order {OrderId}", orderId);
                        foreach (var item in order.OrderItems)
                        {
                            var inventoryRestored = await _inventoryService.AddStockAsync(
                                item.ProductId,
                                item.Quantity,
                                $"Stock returned from cancelled order {order.ReferenceNumber}: {reason}",
                                performedByUserId);

                            if (!inventoryRestored)
                            {
                                result.Warnings.Add($"Failed to restore inventory for product {item.ProductId}");
                            }
                            else
                            {
                                result.InventoryRestored = true;
                            }
                        }

                        _logger.LogDebug("Reverting coupon usage for cancelled order {OrderId}", orderId);
                        if (order.UserId > 0)
                        {
                            var couponIds = await _userCouponService.GetCouponIdsUsedInOrderAsync(orderId);
                            foreach (var couponId in couponIds)
                            {
                                var couponReverted = await _userCouponService.RevertCouponUsageAsync(
                                    order.UserId,
                                    couponId,
                                    orderId);

                                if (!couponReverted)
                                {
                                    result.Warnings.Add($"Failed to revert coupon {couponId} usage");
                                }
                                else
                                {
                                    result.CouponsReverted = true;
                                }
                            }
                        }

                        _logger.LogDebug("Updating order status to Cancelled for order {OrderId}", orderId);
                        order.Status = OrderStatus.Cancelled;
                        order.CancalledAt = DateTime.UtcNow;
                        order.UpdatedAt = DateTime.UtcNow;

                        var orderUpdated = await _orderRepository.UpdateOrderStatusAsync(order.Id, order.Status);
                        if (!orderUpdated)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            result.ErrorMessage = "Failed to update order status to Cancelled";
                            return false;
                        }

                        var committed = await _unitOfWork.CommitTransactionAsync();
                        if (!committed)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            result.ErrorMessage = "Failed to commit cancellation transaction";
                            return false;
                        }

                        result.IsSuccess = true;
                        _logger.LogInformation("Successfully cancelled order {OrderId}", orderId);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error during order cancellation transaction for order {OrderId}", orderId);
                        await _unitOfWork.RollbackTransactionAsync();
                        result.ErrorMessage = $"Cancellation transaction failed: {ex.Message}";
                        return false;
                    }
                });

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during order cancellation for order {OrderId}", orderId);
                result.ErrorMessage = $"Unexpected error: {ex.Message}";
                return result;
            }
        }
    }
}
