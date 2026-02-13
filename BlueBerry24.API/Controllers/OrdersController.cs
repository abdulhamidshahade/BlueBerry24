using AutoMapper;
using BlueBerry24.Application.Authorization.Attributes;
using BlueBerry24.Application.Dtos;
using BlueBerry24.Application.Dtos.OrderDtos;
using BlueBerry24.Application.Services.Interfaces.OrderServiceInterfaces;
using BlueBerry24.Domain.Constants;
using BlueBerry24.Domain.Entities.OrderEntities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlueBerry24.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int? _userId;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService,
                                IHttpContextAccessor httpContextAccessor,
                                IMapper mapper)
        {
            _orderService = orderService;
            _httpContextAccessor = httpContextAccessor;
            _userId = Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            _mapper = mapper;
        }

        [HttpPost]
        [UserAndAbove]
        public async Task<ActionResult<ResponseDto<Order>>> CreateOrder([FromBody] CreateOrderDto request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ResponseDto<Order>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "Invalid request data",
                        Errors = new List<string> { "Order data is required" }
                    });
                }

                var order = await _orderService.CreateOrderFromCartAsync(request.CartId, request);

                if (order == null)
                {
                    return BadRequest(new ResponseDto<Order>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "Order could not be created",
                        Errors = new List<string> { "Cart not found or invalid" }
                    });
                }

                return Ok(new ResponseDto<Order>
                {
                    Data = order,
                    IsSuccess = true,
                    StatusCode = 201,
                    StatusMessage = "Order created successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto<Order>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "Error creating order",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("{id}")]
        [UserAndAbove]
        public async Task<ActionResult<ResponseDto<object>>> GetOrderById(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id, OrderStatus.Pending);
                if (order == null)
                {
                    return NotFound(new ResponseDto<object>
                    {
                        IsSuccess = false,
                        StatusCode = 404,
                        StatusMessage = "Order not found",
                        Data = null
                    });
                }



                return Ok(new ResponseDto<OrderDto>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "Order retrieved successfully",
                    Data = order
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto<object>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "Error retrieving order",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("user/{userId:int}")]
        [UserAndAbove]
        public async Task<ActionResult<ResponseDto<List<OrderDto>>>> GetUserOrders(int userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var orders = await _orderService.GetUserOrdersAsync(userId, page, pageSize);

                return Ok(new ResponseDto<List<OrderDto>>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "User orders retrieved successfully",
                    Data = orders
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto<List<OrderDto>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "Error retrieving user orders",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPut("{orderId}/cancel")]
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto<bool>>> CancelOrder(int orderId, [FromBody] CancelOrderRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request?.Reason))
                {
                    return BadRequest(new ResponseDto<bool>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "Cancellation reason is required"
                    });
                }

                var result = await _orderService.CancelOrderAsync(orderId, request.Reason);
                if (!result)
                {
                    return BadRequest(new ResponseDto<bool>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "Could not cancel the order",
                        Errors = new List<string> { "Order may not exist or is not in a cancellable state" }
                    });
                }

                return Ok(new ResponseDto<bool>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "Order cancelled successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto<bool>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "Error cancelling order",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPut("{orderId}/refund")]
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto<bool>>> RefundOrder(int orderId, [FromBody] RefundOrderRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request?.Reason))
                {
                    return BadRequest(new ResponseDto<bool>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "Refund reason is required"
                    });
                }

                var result = await _orderService.RefundOrderAsync(orderId, request.Reason);
                if (!result)
                {
                    return BadRequest(new ResponseDto<bool>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "Could not refund the order",
                        Errors = new List<string> { "Order may not exist or is not in a refundable state" }
                    });
                }

                return Ok(new ResponseDto<bool>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "Order refunded successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto<bool>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "Error refunding order",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        

        [HttpGet("calculate-totals")]
        [UserAndAbove]
        public async Task<ActionResult<ResponseDto<OrderTotal>>> CalculateOrderTotals([FromQuery] int cartId)
        {
            try
            {
                var totals = await _orderService.CalculateOrderTotalsAsync(cartId);
                if (totals == null)
                {
                    return NotFound(new ResponseDto<OrderTotal>
                    {
                        IsSuccess = false,
                        StatusCode = 404,
                        StatusMessage = "Cart not found"
                    });
                }

                return Ok(new ResponseDto<OrderTotal>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "Order totals calculated successfully",
                    Data = totals
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto<OrderTotal>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "Error calculating order totals",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPut("{orderId}/update-status")]
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto<OrderDto>>> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusRequest request)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    return NotFound(new ResponseDto<OrderDto>
                    {
                        IsSuccess = false,
                        StatusCode = 404,
                        StatusMessage = "Order not found"
                    });
                }

                var mappedOrder = _mapper.Map<Order>(order);

                var result = await _orderService.UpdateOrderStatusAsync(mappedOrder, request.NewStatus);
                if (!result)
                {
                    return BadRequest(new ResponseDto<OrderDto>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "Failed to update order status",
                        Errors = new List<string> { "Invalid status transition or order state" }
                    });
                }

                return Ok(new ResponseDto<OrderDto>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "Order status updated successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto<OrderDto>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "Error updating order status",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("reference/{referenceNumber}")]
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto<Order>>> GetOrderByReference(string referenceNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(referenceNumber))
                {
                    return BadRequest(new ResponseDto<Order>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "Reference number is required"
                    });
                }

                var order = await _orderService.GetOrderByReferenceNumberAsync(referenceNumber);
                if (order == null)
                {
                    return NotFound(new ResponseDto<Order>
                    {
                        IsSuccess = false,
                        StatusCode = 404,
                        StatusMessage = "Order not found"
                    });
                }

                return Ok(new ResponseDto<Order>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "Order retrieved successfully",
                    Data = order
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto<Order>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "Error retrieving order",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("status/{status}")]
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto<List<Order>>>> GetOrdersByStatus(OrderStatus status, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var orders = await _orderService.GetOrdersByStatusAsync(status, page, pageSize);

                return Ok(new ResponseDto<List<Order>>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "Orders retrieved successfully",
                    Data = orders
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto<List<Order>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "Error retrieving orders",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("admin/all")]
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto<List<OrderDto>>>> GetAllOrdersForAdmin([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync(page, pageSize);

                return Ok(new ResponseDto<List<OrderDto>>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "All orders retrieved successfully",
                    Data = orders
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto<List<OrderDto>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "Error retrieving all orders",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPut("{orderId}/process")]
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto<bool>>> ProcessOrder(int orderId)
        {
            try
            {
                var result = await _orderService.ProcessOrderAsync(orderId);
                if (!result)
                {
                    return BadRequest(new ResponseDto<bool>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "Could not process the order",
                        Errors = new List<string> { "Order may not exist or is not in a processable state" }
                    });
                }

                return Ok(new ResponseDto<bool>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "Order processed successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto<bool>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "Error processing order",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }

}
