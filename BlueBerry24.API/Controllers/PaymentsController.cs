using AutoMapper;
using BlueBerry24.Application.Authorization.Attributes;
using BlueBerry24.Application.Dtos.PaymentDtos;
using BlueBerry24.Application.Services.Interfaces.OrderServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.PaymentServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces;
using BlueBerry24.Domain.Constants;
using BlueBerry24.Domain.Entities.OrderEntities;
using Microsoft.AspNetCore.Mvc;

namespace BlueBerry24.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : BaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public PaymentsController(ILogger<PaymentsController> logger, IPaymentService paymentService, ICartService cartService, IOrderService orderService, IMapper mapper)
            : base(logger)
        {
            _paymentService = paymentService;
            _cartService = cartService;
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost("process")]
        [UserAndAbove]
        public async Task<IActionResult> ProcessPayment([FromBody] CreatePaymentDto createPaymentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();
                var sessionId = GetSessionId();

                var result = await _paymentService.ProcessPaymentAsync(createPaymentDto, userId, sessionId);

                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }

                var cart = await _cartService.GetCartByUserIdAsync(userId.Value);

                //await _cartService.ConvertCartAsync(cart.Id);
                var order = await _orderService.GetOrderByIdAsync(createPaymentDto.OrderId.Value);

                var mappedOrder = _mapper.Map<Order>(order);

                await _orderService.UpdateOrderStatusAsync(mappedOrder, OrderStatus.Completed);
                await _orderService.UpdateOrderPaymentStatusAsync(mappedOrder, PaymentStatus.Completed);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment");
                return StatusCode(500, "An error occurred while processing payment");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            try
            {
                var result = await _paymentService.GetPaymentByIdAsync(id);

                if (!result.IsSuccess)
                {
                    return NotFound(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payment by ID: {PaymentId}", id);
                return StatusCode(500, "An error occurred while retrieving payment");
            }
        }

        [HttpGet("transaction/{transactionId}")]
        public async Task<IActionResult> GetPaymentByTransactionId(string transactionId)
        {
            try
            {
                var result = await _paymentService.GetPaymentByTransactionIdAsync(transactionId);

                if (!result.IsSuccess)
                {
                    return NotFound(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payment by transaction ID: {TransactionId}", transactionId);
                return StatusCode(500, "An error occurred while retrieving payment");
            }
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetPaymentByOrderId(int orderId)
        {
            try
            {
                var result = await _paymentService.GetPaymentByOrderIdAsync(orderId);

                if (!result.IsSuccess)
                {
                    return NotFound(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payment by order ID: {OrderId}", orderId);
                return StatusCode(500, "An error occurred while retrieving payment");
            }
        }

        [HttpGet]
        [AdminAndAbove]
        public async Task<IActionResult> GetAllPayments()
        {
            try
            {
                var result = await _paymentService.GetAllPaymentsAsync();

                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all payments");
                return StatusCode(500, "An error occurred while retrieving payments");
            }
        }

        [HttpGet("user/{userId}")]
        [UserAndAbove]
        public async Task<IActionResult> GetPaymentsByUserId(int userId)
        {
            try
            {
                var currentUserId = GetCurrentUserId();

                // Users can only access their own payments unless they're admin
                if (currentUserId != userId && !User.IsInRole("User"))
                {
                    return Forbid("You can only access your own payments");
                }

                var result = await _paymentService.GetPaymentsByUserIdAsync(userId);

                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payments by user ID: {UserId}", userId);
                return StatusCode(500, "An error occurred while retrieving user payments");
            }
        }

        [HttpGet("my-payments")]
        [UserAndAbove]
        public async Task<IActionResult> GetMyPayments(int page = 1, int pageSize = 10)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (!userId.HasValue)
                {
                    return Unauthorized("User not authenticated");
                }

                var result = await _paymentService.GetPaginatedPaymentsByUserIdAsync(userId.Value, page, pageSize);

                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting current user payments");
                return StatusCode(500, "An error occurred while retrieving your payments");
            }
        }

        [HttpGet("status/{status}")]
        [AdminAndAbove]
        public async Task<IActionResult> GetPaymentsByStatus(PaymentStatus status)
        {
            try
            {
                var result = await _paymentService.GetPaymentsByStatusAsync(status);

                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payments by status: {Status}", status);
                return StatusCode(500, "An error occurred while retrieving payments by status");
            }
        }

        [HttpGet("date-range")]
        [AdminAndAbove]
        public async Task<IActionResult> GetPaymentsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = await _paymentService.GetPaymentsByDateRangeAsync(startDate, endDate);

                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payments by date range");
                return StatusCode(500, "An error occurred while retrieving payments by date range");
            }
        }

        [HttpGet("paginated")]
        [AdminAndAbove]
        public async Task<IActionResult> GetPaginatedPayments(int page = 1, int pageSize = 50)
        {
            try
            {
                var result = await _paymentService.GetPaginatedPaymentsAsync(page, pageSize);

                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paginated payments");
                return StatusCode(500, "An error occurred while retrieving paginated payments");
            }
        }

        [HttpPut("{id}/status")]
        [AdminAndAbove]
        public async Task<IActionResult> UpdatePaymentStatus(int id, [FromBody] UpdatePaymentStatusDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _paymentService.UpdatePaymentStatusAsync(id, updateDto.Status, updateDto.Notes);

                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating payment status for ID: {PaymentId}", id);
                return StatusCode(500, "An error occurred while updating payment status");
            }
        }

        [HttpPost("{id}/refund")]
        [AdminAndAbove]
        public async Task<IActionResult> RefundPayment(int id, [FromBody] RefundPaymentDto refundDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _paymentService.RefundPaymentAsync(id, refundDto.RefundAmount, refundDto.Reason);

                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refunding payment for ID: {PaymentId}", id);
                return StatusCode(500, "An error occurred while processing refund");
            }
        }

        [HttpDelete("{id}")]
        [AdminAndAbove]
        public async Task<IActionResult> DeletePayment(int id)
        {
            try
            {
                var result = await _paymentService.DeletePaymentAsync(id);

                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting payment for ID: {PaymentId}", id);
                return StatusCode(500, "An error occurred while deleting payment");
            }
        }

        [HttpGet("stats/count")]
        [AdminAndAbove]
        public async Task<IActionResult> GetTotalPaymentCount()
        {
            try
            {
                var result = await _paymentService.GetTotalPaymentCountAsync();

                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total payment count");
                return StatusCode(500, "An error occurred while retrieving payment count");
            }
        }

        [HttpGet("stats/amount/total")]
        [AdminAndAbove]
        public async Task<IActionResult> GetTotalAmountByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = await _paymentService.GetTotalAmountByDateRangeAsync(startDate, endDate);

                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total amount by date range");
                return StatusCode(500, "An error occurred while retrieving total amount");
            }
        }

        [HttpGet("search")]
        [AdminAndAbove]
        public async Task<IActionResult> SearchPayments(string searchTerm, int page = 1, int pageSize = 50)
        {
            try
            {
                var result = await _paymentService.SearchPaymentsAsync(searchTerm, page, pageSize);

                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching payments with term: {SearchTerm}", searchTerm);
                return StatusCode(500, "An error occurred while searching payments");
            }
        }

        [HttpPost("{transactionId}/verify")]
        [AdminAndAbove]
        public async Task<IActionResult> VerifyPaymentWithProvider(string transactionId)
        {
            try
            {
                var result = await _paymentService.VerifyPaymentWithProviderAsync(transactionId);

                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying payment with transaction ID: {TransactionId}", transactionId);
                return StatusCode(500, "An error occurred while verifying payment");
            }
        }
    }

    public class UpdatePaymentStatusDto
    {
        public PaymentStatus Status { get; set; }
        public string? Notes { get; set; }
    }

    public class RefundPaymentDto
    {
        public decimal? RefundAmount { get; set; }
        public string? Reason { get; set; }
    }
}
