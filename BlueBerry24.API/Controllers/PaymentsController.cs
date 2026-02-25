using AutoMapper;
using BlueBerry24.Application.Authorization.Attributes;
using BlueBerry24.Application.Dtos.PaymentDtos;
using BlueBerry24.Application.Services.Interfaces.OrderServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.PaymentServiceInterfaces;
using BlueBerry24.Domain.Constants;
using BlueBerry24.Domain.Entities.OrderEntities;
using Microsoft.AspNetCore.Mvc;

namespace BlueBerry24.API.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentsController : BaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public PaymentsController(
            IPaymentService paymentService, 
            IOrderService orderService, 
            IMapper mapper)
        {
            _paymentService = paymentService;
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

                if (createPaymentDto.OrderId.HasValue)
                {
                    var order = await _orderService.GetOrderByIdAsync(createPaymentDto.OrderId.Value);
                    if (order != null)
                    {
                        var mappedOrder = _mapper.Map<Order>(order);
                        await _orderService.UpdateOrderStatusAsync(mappedOrder, OrderStatus.Completed);
                        await _orderService.UpdateOrderPaymentStatusAsync(mappedOrder, PaymentStatus.Completed);
                    }
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { IsSuccess = false, StatusCode = 500, StatusMessage = "An error occurred while processing payment", Errors = new[] { ex.Message } });
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
                return StatusCode(500, new { IsSuccess = false, StatusCode = 500, StatusMessage = "An error occurred while retrieving payment", Errors = new[] { ex.Message } });
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
                return StatusCode(500, new { IsSuccess = false, StatusCode = 500, StatusMessage = "An error occurred while retrieving payment", Errors = new[] { ex.Message } });
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
                return StatusCode(500, new { IsSuccess = false, StatusCode = 500, StatusMessage = "An error occurred while retrieving payment", Errors = new[] { ex.Message } });
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
                return StatusCode(500, new { IsSuccess = false, StatusCode = 500, StatusMessage = "An error occurred while retrieving payments", Errors = new[] { ex.Message } });
            }
        }

        [HttpGet("user/{userId}")]
        [UserAndAbove]
        public async Task<IActionResult> GetPaymentsByUserId(int userId)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                if (currentUserId != userId && !User.IsInRole("Admin"))
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
                return StatusCode(500, new { IsSuccess = false, StatusCode = 500, StatusMessage = "An error occurred while retrieving user payments", Errors = new[] { ex.Message } });
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
                    return Unauthorized(new { IsSuccess = false, StatusCode = 401, StatusMessage = "User not authenticated" });
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
                return StatusCode(500, new { IsSuccess = false, StatusCode = 500, StatusMessage = "An error occurred while retrieving your payments", Errors = new[] { ex.Message } });
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
                return StatusCode(500, new { IsSuccess = false, StatusCode = 500, StatusMessage = "An error occurred while retrieving payments by status", Errors = new[] { ex.Message } });
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
                return StatusCode(500, new { IsSuccess = false, StatusCode = 500, StatusMessage = "An error occurred while retrieving payments by date range", Errors = new[] { ex.Message } });
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
                return StatusCode(500, new { IsSuccess = false, StatusCode = 500, StatusMessage = "An error occurred while retrieving paginated payments", Errors = new[] { ex.Message } });
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
                return StatusCode(500, new { IsSuccess = false, StatusCode = 500, StatusMessage = "An error occurred while updating payment status", Errors = new[] { ex.Message } });
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
                return StatusCode(500, new { IsSuccess = false, StatusCode = 500, StatusMessage = "An error occurred while processing refund", Errors = new[] { ex.Message } });
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
                return StatusCode(500, new { IsSuccess = false, StatusCode = 500, StatusMessage = "An error occurred while deleting payment", Errors = new[] { ex.Message } });
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
                return StatusCode(500, new { IsSuccess = false, StatusCode = 500, StatusMessage = "An error occurred while retrieving payment count", Errors = new[] { ex.Message } });
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
                return StatusCode(500, new { IsSuccess = false, StatusCode = 500, StatusMessage = "An error occurred while retrieving total amount", Errors = new[] { ex.Message } });
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
                return StatusCode(500, new { IsSuccess = false, StatusCode = 500, StatusMessage = "An error occurred while searching payments", Errors = new[] { ex.Message } });
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
                return StatusCode(500, new { IsSuccess = false, StatusCode = 500, StatusMessage = "An error occurred while verifying payment", Errors = new[] { ex.Message } });
            }
        }
    }
}
