using BlueBerry24.Application.Services.Interfaces.CouponServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.InventoryServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.OrchestrationServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.PaymentServiceInterfaces;
using BlueBerry24.Domain.Constants;
using BlueBerry24.Domain.Repositories;
using BlueBerry24.Domain.Repositories.OrderInterfaces;
using BlueBerry24.Domain.Repositories.PaymentInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlueBerry24.Application.Services.Concretes.OrchestrationServiceConcretes
{
    public class RefundOrchestrationService : IRefundOrchestrationService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IInventoryService _inventoryService;
        private readonly IUserCouponService _userCouponService;
        private readonly IPaymentService _paymentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RefundOrchestrationService> _logger;

        public RefundOrchestrationService(
            IOrderRepository orderRepository,
            IPaymentRepository paymentRepository,
            IInventoryService inventoryService,
            IUserCouponService userCouponService,
            IPaymentService paymentService,
            IUnitOfWork unitOfWork,
            ILogger<RefundOrchestrationService> logger)
        {
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
            _inventoryService = inventoryService;
            _userCouponService = userCouponService;
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<RefundResult> ProcessRefundAsync(int orderId, string reason, decimal? refundAmount = null, int? performedByUserId = null)
        {
            var result = new RefundResult();

            try
            {
                _logger.LogInformation("Starting refund process for order {OrderId}", orderId);

                var order = await _orderRepository.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    result.ErrorMessage = "Order not found";
                    return result;
                }

                if (order.Status != OrderStatus.Completed && order.Status != OrderStatus.Delivered)
                {
                    result.ErrorMessage = $"Cannot refund order with status {order.Status}. Only Completed or Delivered orders can be refunded.";
                    return result;
                }

                var payment = await _paymentRepository.GetByOrderIdAsync(orderId);
                if (payment == null)
                {
                    result.ErrorMessage = "Payment not found for this order";
                    return result;
                }

                if (payment.Status != PaymentStatus.Completed)
                {
                    result.ErrorMessage = $"Cannot refund payment with status {payment.Status}. Only completed payments can be refunded.";
                    return result;
                }

                var amountToRefund = refundAmount ?? order.Total;
                if (amountToRefund > order.Total)
                {
                    result.ErrorMessage = "Refund amount cannot exceed order total";
                    return result;
                }

                var strategy = _unitOfWork.BeginTransactionAsyncStrategy();
                var transactionResult = await strategy.ExecuteAsync(async () =>
                {
                    await _unitOfWork.BeginTransactionAsync();

                    try
                    {
                        _logger.LogDebug("Processing payment refund for order {OrderId}", orderId);
                        var paymentRefundResult = await _paymentService.RefundPaymentAsync(payment.Id, amountToRefund, reason);
                        
                        if (!paymentRefundResult.IsSuccess || paymentRefundResult.Data?.Success != true)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            result.ErrorMessage = "Payment refund failed";
                            return false;
                        }

                        result.PaymentRefunded = true;
                        result.RefundedAmount = amountToRefund;

                        _logger.LogDebug("Restoring inventory for refunded order {OrderId}", orderId);
                        foreach (var item in order.OrderItems)
                        {
                            var inventoryRestored = await _inventoryService.AddStockAsync(
                                item.ProductId,
                                item.Quantity,
                                $"Stock returned from refunded order {order.ReferenceNumber}: {reason}",
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

                        _logger.LogDebug("Reverting coupon usage for refunded order {OrderId}", orderId);
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

                        _logger.LogDebug("Updating order status to Refunded for order {OrderId}", orderId);
                        order.Status = OrderStatus.Refunded;
                        order.UpdatedAt = DateTime.UtcNow;

                        var orderUpdated = await _orderRepository.UpdateOrderStatusAsync(order.Id, order.Status);
                        if (!orderUpdated)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            result.ErrorMessage = "Failed to update order status to Refunded";
                            return false;
                        }

                        var committed = await _unitOfWork.CommitTransactionAsync();
                        if (!committed)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            result.ErrorMessage = "Failed to commit refund transaction";
                            return false;
                        }

                        result.IsSuccess = true;
                        _logger.LogInformation("Successfully processed refund for order {OrderId}, amount {RefundAmount}",
                            orderId, amountToRefund);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error during refund transaction for order {OrderId}", orderId);
                        await _unitOfWork.RollbackTransactionAsync();
                        result.ErrorMessage = $"Refund transaction failed: {ex.Message}";
                        return false;
                    }
                });

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during refund for order {OrderId}", orderId);
                result.ErrorMessage = $"Unexpected error: {ex.Message}";
                return result;
            }
        }
    }
}
