using BlueBerry24.Application.Authorization.Attributes;
using BlueBerry24.Application.Dtos;
using BlueBerry24.Application.Dtos.ShoppingCartDtos;
using BlueBerry24.Application.Services.Interfaces.CouponServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.InventoryServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.OrderServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces;
using BlueBerry24.Domain.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlueBerry24.API.Controllers
{
    [Route("api/shopping-carts")]
    [ApiController]
    public class ShoppingCartsController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int? _userId;
        private readonly IUserCouponService _userCouponService;
        private readonly ICartService _cartService;
        private readonly IInventoryService _inventoryService;
        private readonly IOrderService _orderService;

        public ShoppingCartsController(
            IHttpContextAccessor httpContextAccessor,
            IUserCouponService userCouponService,
            ICartService cartService,
            IInventoryService inventoryService,
            IOrderService orderService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userId = Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            _userCouponService = userCouponService;
            _cartService = cartService;
            _inventoryService = inventoryService;
            _orderService = orderService;
        }

        [HttpGet("user-id")]
        public async Task<ActionResult<ResponseDto<CartDto>>> GetCartByUserId()
        {
            try
            {
                //TODO to fix
                var cart = await _cartService.GetCartByUserIdAsync(GetCurrentUserId().Value);
                if (cart == null)
                {
                    cart = await _cartService.CreateCartAsync(GetCurrentUserId(), null);

                    if (cart == null)
                    {
                        return NotFound(new ResponseDto<CartDto>
                        {
                            IsSuccess = false,
                            StatusCode = 404,
                            StatusMessage = "Cart not found"
                        });
                    }

                }

                return Ok(new ResponseDto<CartDto>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "Cart retrieved successfully",
                    Data = cart
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto<CartDto>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "Error retrieving cart",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("session-id")]

        public async Task<ActionResult<ResponseDto<CartDto>>> GetCartBySessionId()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(GetSessionId()))
                {
                    return BadRequest(new ResponseDto<CartDto>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "Session ID is required"
                    });
                }

                int? userId = GetCurrentUserId();
                string? sessionIdd = GetSessionId();

                var cart = await _cartService.GetCartBySessionIdAsync(GetSessionId(), CartStatus.Active);
                if (cart == null)
                {
                    cart = await _cartService.CreateCartAsync(null, GetSessionId());

                    if (cart == null)
                    {
                        return NotFound(new ResponseDto<CartDto>
                        {
                            IsSuccess = false,
                            StatusCode = 404,
                            StatusMessage = "Cart not found for session"
                        });
                    }

                }

                return Ok(new ResponseDto<CartDto>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "Cart retrieved successfully",
                    Data = cart
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto<CartDto>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "Error retrieving cart",
                    Errors = new List<string> { ex.Message }
                });
            }
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<ResponseDto<CartDto>>> GetCartByCartId(int id)
        {
            try
            {
                var cart = await _cartService.GetCartByIdAsync(id, CartStatus.Active);
                if (cart == null)
                {
                    return NotFound(new ResponseDto<CartDto>
                    {
                        IsSuccess = false,
                        StatusCode = 404,
                        StatusMessage = "Cart not found for id"
                    });
                }

                return Ok(new ResponseDto<CartDto>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "Cart retrieved successfully",
                    Data = cart
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto<CartDto>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "Error retrieving cart",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost("create")]

        public async Task<ActionResult<ResponseDto<CartDto>>> CreateCart()
        {
            try
            {
                if (_userId == null && string.IsNullOrWhiteSpace(GetSessionId()))
                {
                    return BadRequest(new ResponseDto<CartDto>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "Either userId or sessionId must be provided"
                    });
                }

                var cart = await _cartService.CreateCartAsync(GetCurrentUserId(), GetSessionId());
                if (cart == null)
                {
                    return BadRequest(new ResponseDto<CartDto>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "Cart could not be created"
                    });
                }

                return Ok(new ResponseDto<CartDto>
                {
                    IsSuccess = true,
                    StatusCode = 201,
                    StatusMessage = "Cart created successfully",
                    Data = cart
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto<CartDto>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "Error creating cart",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost("add-item")]
        public async Task<ActionResult<ResponseDto<CartDto>>> AddItemToCart([FromBody] AddItemRequest itemRequest)
        {
            try
            {
                if (itemRequest.Quantity <= 0)
                {
                    return BadRequest(new ResponseDto<CartDto>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "Quantity must be greater than 0"
                    });
                }

                var isInStock = await _inventoryService.IsInStockAsync(itemRequest.ProductId, itemRequest.Quantity);
                if (!isInStock)
                {
                    return BadRequest(new ResponseDto<CartDto>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "Insufficient stock for the requested quantity",
                        Errors = new List<string> { $"Product {itemRequest.ProductId} does not have {itemRequest.Quantity} items in stock" }
                    });
                }

                var updatedCart = await _cartService.AddItemAsync(itemRequest.CartId, GetCurrentUserId(), itemRequest.SessionId, itemRequest.ProductId, itemRequest.Quantity);
                if (updatedCart == null)
                {
                    return BadRequest(new ResponseDto<CartDto>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "Failed to add item to cart",
                        Errors = new List<string> { "Cart not found or product invalid" }
                    });
                }

                return Ok(new ResponseDto<CartDto>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "Item added to cart successfully",
                    Data = updatedCart
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto<CartDto>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "Error adding item to cart",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPut("{cartId}/update")]
        public async Task<ActionResult<ResponseDto<CartDto>>> UpdateItemQuantity([FromBody] AddItemRequest itemRequest)
        {
            try
            {
                if (itemRequest.Quantity < 0)
                {
                    return BadRequest(new ResponseDto<CartDto>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "Quantity cannot be negative"
                    });
                }

                if (itemRequest.Quantity == 0)
                {
                    var removeResult = await RemoveItemFromCart(itemRequest.CartId, itemRequest.ProductId);
                    return Ok(new ResponseDto<CartDto>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        StatusMessage = "Item quantity updated successfully",
                        Data = new CartDto()
                    });
                }

                var isInStock = await _inventoryService.IsInStockAsync(itemRequest.ProductId, itemRequest.Quantity);
                if (!isInStock)
                {
                    return BadRequest(new ResponseDto<CartDto>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "Insufficient stock for the requested quantity",
                        Errors = new List<string> { $"Product {itemRequest.ProductId} does not have {itemRequest.Quantity} items in stock" }
                    });
                }

                var updatedCart = await _cartService.UpdateItemQuantityAsync(itemRequest.CartId, itemRequest.UserId, itemRequest.SessionId, itemRequest.ProductId, itemRequest.Quantity);
                if (updatedCart == null)
                {
                    return BadRequest(new ResponseDto<CartDto>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "Failed to update item quantity",
                        Errors = new List<string> { "Cart or item not found" }
                    });
                }

                return Ok(new ResponseDto<CartDto>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "Item quantity updated successfully",
                    Data = updatedCart
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto<CartDto>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "Error updating item quantity",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpDelete("{cartId}/remove/{productId}")]
        public async Task<ActionResult<ResponseDto<bool>>> RemoveItemFromCart(int cartId, int productId)
        {
            try
            {
                var success = await _cartService.RemoveItemAsync(cartId, GetCurrentUserId(), GetSessionId(), productId);
                if (!success)
                {
                    return NotFound(new ResponseDto<bool>
                    {
                        IsSuccess = false,
                        StatusCode = 404,
                        StatusMessage = "Item not found in cart"
                    });
                }

                return Ok(new ResponseDto<bool>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "Item removed from cart successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto<bool>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "Error removing item from cart",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpDelete("{cartId}/clear")]
        public async Task<ActionResult<ResponseDto<bool>>> ClearCart(int cartId)
        {
            try
            {
                var success = await _cartService.ClearCartAsync(cartId, GetCurrentUserId(), GetSessionId());
                if (!success)
                {
                    return BadRequest(new ResponseDto<bool>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "Failed to clear the cart",
                        Errors = new List<string> { "Cart not found or already empty" }
                    });
                }

                return Ok(new ResponseDto<bool>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "Cart cleared successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto<bool>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "Error clearing cart",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost("{cartId}/complete")]
        public async Task<ActionResult<ResponseDto<bool>>> CompleteCart(int cartId)
        {
            try
            {
                var success = await _cartService.CompleteCartAsync(cartId, GetCurrentUserId());
                if (!success)
                {
                    return BadRequest(new ResponseDto<bool>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "Failed to complete the cart",
                        Errors = new List<string> { "Cart not found or cannot be completed" }
                    });
                }

                return Ok(new ResponseDto<bool>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "Cart completed successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto<bool>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "Error completing cart",
                    Errors = new List<string> { ex.Message }
                });
            }
        }



        [HttpGet("{cartId}/item/{productId}")]
        public async Task<ActionResult<ResponseDto<CartItemDto>>> GetCartItem(int cartId, int productId)
        {
            try
            {
                var item = await _cartService.GetItemAsync(cartId, productId);
                if (item == null)
                {
                    return NotFound(new ResponseDto<CartItemDto>
                    {
                        IsSuccess = false,
                        StatusCode = 404,
                        StatusMessage = "Item not found in cart"
                    });
                }

                return Ok(new ResponseDto<CartItemDto>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "Cart item retrieved successfully",
                    Data = item
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto<CartItemDto>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "Error retrieving cart item",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost("{cartId}/apply-coupon")]
        [UserAndAbove]
        public async Task<ActionResult<ResponseDto<CartDto>>> ApplyCoupon(int cartId, [FromBody] ApplyCouponRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request?.CouponCode))
                {
                    return BadRequest(new ResponseDto<CartDto>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "Coupon code is required"
                    });
                }

                var cart = await _cartService.ApplyCouponAsync(cartId, GetCurrentUserId(), request.CouponCode);
                if (cart == null)
                {
                    return BadRequest(new ResponseDto<CartDto>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "Failed to apply coupon",
                        Errors = new List<string> { "Invalid coupon code or cart not found" }
                    });
                }

                return Ok(new ResponseDto<CartDto>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "Coupon applied successfully",
                    Data = cart
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto<CartDto>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Error applying coupon",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpDelete("{cartId}/remove-coupon/{couponId}")]
        public async Task<ActionResult<ResponseDto<CartDto>>> RemoveCoupon(int cartId, int couponId)
        {
            try
            {
                var cart = await _cartService.RemoveCouponAsync(cartId, GetCurrentUserId(), GetSessionId(), couponId);
                if (cart == null)
                {
                    return BadRequest(new ResponseDto<CartDto>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "Failed to remove coupon",
                        Errors = new List<string> { "Coupon not found in cart or cart not found" }
                    });
                }

                return Ok(new ResponseDto<CartDto>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "Coupon removed successfully",
                    Data = cart
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto<CartDto>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Error removing coupon",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost("{cartId}/checkout")]
        public async Task<ActionResult<ResponseDto<object>>> CheckoutCart(int cartId, [FromBody] CheckoutRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ResponseDto<object>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "Checkout data is required"
                    });
                }

                var cart = await _cartService.GetCartByIdAsync(cartId, CartStatus.Active);
                if (cart == null)
                {
                    return NotFound(new ResponseDto<object>
                    {
                        IsSuccess = false,
                        StatusCode = 404,
                        StatusMessage = "Cart not found"
                    });
                }

                if (cart.CartItems == null || cart.CartItems.Count == 0)
                {
                    return BadRequest(new ResponseDto<object>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "Cart is empty"
                    });
                }

                // Validate stock for all items before proceeding
                foreach (var item in cart.CartItems)
                {
                    var isInStock = await _inventoryService.IsInStockAsync(item.ProductId, item.Quantity);
                    if (!isInStock)
                    {
                        return BadRequest(new ResponseDto<object>
                        {
                            IsSuccess = false,
                            StatusCode = 400,
                            StatusMessage = $"Insufficient stock for product ID {item.ProductId}. Requested: {item.Quantity}"
                        });
                    }
                }

                var createOrderDto = new Application.Dtos.OrderDtos.CreateOrderDto
                {
                    UserId = GetCurrentUserId() ?? 0,
                    CartId = cartId,
                    CustomerEmail = request.CustomerEmail,
                    CustomerPhone = request.CustomerPhone,
                    ShippingName = $"{request.ShippingName}",
                    ShippingAddressLine1 = request.ShippingAddressLine1,
                    ShippingAddressLine2 = request.ShippingAddressLine2,
                    ShippingCity = request.ShippingCity,
                    ShippingState = request.ShippingState,
                    ShippingPostalCode = request.ShippingPostalCode,
                    ShippingCountry = request.ShippingCity ?? "US",
                };

                var order = await _orderService.CreateOrderFromCartAsync(cartId, createOrderDto);

                if (order == null)
                {
                    return BadRequest(new ResponseDto<object>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        StatusMessage = "Failed to create order"
                    });
                }

                await _cartService.CompleteCartAsync(cartId, GetCurrentUserId());

                return Ok(new ResponseDto<object>
                {
                    IsSuccess = true,
                    StatusCode = 201,
                    StatusMessage = "Order created successfully",
                    Data = new
                    {
                        id = order.Id,
                        orderNumber = "11",
                        total = 10,
                        status = "yes"
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto<object>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "Error processing checkout",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

    }

}