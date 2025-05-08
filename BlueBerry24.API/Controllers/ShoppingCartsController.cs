using BlueBerry24.Application.Dtos;
using BlueBerry24.Application.Dtos.ShoppingCartDtos;
using BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces.Cache;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlueBerry24.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartsController : ControllerBase
    {
        private readonly ILogger<ShoppingCartsController> _logger;

        private readonly ICartHeaderService _cartHeaderService;
        private readonly ICartItemService _cartItemService;
        private readonly ICartHeaderCacheService _cartHeaderCacheService;
        private readonly ICartItemCacheService _cartItemCacheService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int? _userId;
        private readonly ICartCacheService _cartCacheService;

        public ShoppingCartsController(ILogger<ShoppingCartsController> logger,
                                       ICartHeaderService cartHeaderService,
                                       ICartItemService cartItemService,
                                       ICartHeaderCacheService cartHeaderCacheService,
                                       ICartItemCacheService cartItemCacheService,
                                       IHttpContextAccessor httpContextAccessor,
                                       ICartCacheService cartCacheService)
        {
            _logger = logger;
            _cartHeaderService = cartHeaderService;
            _cartItemService = cartItemService;
            _cartHeaderCacheService = cartHeaderCacheService;
            _cartItemCacheService = cartItemCacheService;
            _httpContextAccessor = httpContextAccessor;
            _userId = Convert.ToInt16(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            _cartCacheService = cartCacheService;
        }


        [HttpGet]
        [Route("{userId}")]
        public async Task<ActionResult<ResponseDto>> GetCurrentUserCart(int userId)
        {
            //if (string.IsNullOrEmpty(userId))
            //{
            //    throw new ArgumentException("userId parameter is null or empty!", nameof(userId));
            //}

            if (_userId != userId)
            {
                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "The requested shopping cart is not match with the current authenticated user."
                });
            }

            var cart = await _cartCacheService.GetCartAsync(userId);

            if (cart == null)
            {
                return NotFound(new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    StatusMessage = "The cart is not found"
                });
            }

            return Ok(new ResponseDto
            {
                Data = cart,
                IsSuccess = true,
                StatusCode = 200
            });
        }



        [HttpPost]
        [Route("add-product")]
        public async Task<ActionResult<ResponseDto>> AddProductToShoppingCart(int userId, CartItemDto cartItemDto)
        {
            var isAdded = await _cartItemCacheService.
                AddItemAsync(cartItemDto, userId);

            if (isAdded)
            {
                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Failed to add the item to the shopping cart"
                });
            }
            else
            {
                return Ok(new ResponseDto
                {
                    IsSuccess = true,
                    StatusMessage = "Item added successfully",
                    StatusCode = 200
                });
            }
        }


        [HttpPut]
        [Route("increase-item/{userId}")]
        public async Task<ActionResult<ResponseDto>> IncreaseItem(int userId, CartItemDto cartItemDto)
        {
            //if (string.IsNullOrEmpty(userId))
            //{
            //    return new ResponseDto
            //    {
            //        IsSuccess = false,
            //        StatusCode = 400,
            //        StatusMessage = "User id can't be empty or null"
            //    };
            //}


            var itemToIncrease = await _cartItemCacheService.IncreaseItemAsync(cartItemDto, userId);

            if (itemToIncrease)
            {
                return new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "Item increased successfully"
                };
            }
            else
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Failed to increase item"
                };
            }
        }


        [HttpPut]
        [Route("decrease-item/{userId}")]
        public async Task<ActionResult<ResponseDto>> DecreaseItem(int userId, CartItemDto cartItemDto)
        {
            //if (string.IsNullOrEmpty(userId))
            //{
            //    return new ResponseDto
            //    {
            //        IsSuccess = false,
            //        StatusCode = 400,
            //        StatusMessage = "User id can't be empty or null"
            //    };
            //}


            var itemToDecrease = await _cartItemCacheService.DecreaseItemAsync(cartItemDto, userId);

            if (itemToDecrease)
            {
                return new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "Item decreased successfully"
                };
            }
            else
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Failed to decrease item"
                };
            }
        }


        [HttpDelete]
        [Route("delete-item{userId}")]
        public async Task<ActionResult<ResponseDto>> RemoveItem(int userId, CartItemDto cartItemDto)
        {
            //if (string.IsNullOrEmpty(userId))
            //{
            //    return new ResponseDto
            //    {
            //        IsSuccess = false,
            //        StatusCode = 400,
            //        StatusMessage = "User Id can't be empty or null"
            //    };
            //}

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var itemToRemove = await _cartItemCacheService.RemoveItemAsync(cartItemDto, userId);

            if (itemToRemove)
            {
                return new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "Item deleted successfully"
                };
            }
            else
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Failed to deleted item"
                };
            }
        }


        [HttpDelete]
        [Route("{userId}")]
        public async Task<ActionResult<ResponseDto>> DeleteShoppingCart(int userId, [FromQuery] int headerId)
        {
            //if (string.IsNullOrEmpty(userId))
            //{
            //    return new ResponseDto
            //    {
            //        IsSuccess = false,
            //        StatusCode = 400,
            //        StatusMessage = "User Id can't be empty or null"
            //    };
            //}


            var cartToDelete = await _cartCacheService.DeleteCartAsync(userId);

            if (cartToDelete)
            {
                return new ResponseDto
                {
                    IsSuccess = true,
                    StatusMessage = "Cart Deleted Successfully",
                    StatusCode = 200
                };
            }
            else
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Failed deleting cart"
                };
            }
        }





        //[HttpPut]
        //[Route("redeem-coupon/{couponCode}")]
        //public async Task<ActionResult<ResponseDto>> RedeemCoupon(string couponCode, 
        //                                                          RedeemCouponRequestDto redeemCouponDto)
        //{

        //    if (string.IsNullOrEmpty(couponCode))
        //    {
        //        return new ResponseDto
        //        {
        //            StatusCode = 400,
        //            IsSuccess = false,
        //            StatusMessage = "Coupon code can't be null or empty value"
        //        };
        //    }


        //    var cart = await _cartService.ExistsByHeaderIdAsync(redeemCouponDto.UserId, redeemCouponDto.HeaderId);

        //    if (cart == null)
        //    {
        //        return NotFound(new ResponseDto
        //        {
        //            IsSuccess = false,
        //            StatusCode = 404,
        //            StatusMessage = "The cart is not found"
        //        });
        //    }

        //    var cartDto = _mapper.Map<CartHeaderDto>(cart);

        //    var couponToRedeem = await _cartService.RedeemCouponAsync(redeemCouponDto.UserId, cartDto);


        //    if (couponToRedeem)
        //    {
        //        return BadRequest(new ResponseDto
        //        {
        //            IsSuccess = false,
        //            StatusCode = 400,
        //            StatusMessage = "The total price doesn't match the minimum required for coupon"
        //        });
        //    }
        //    else
        //    {

        //        return Ok(new ResponseDto
        //        {
        //            IsSuccess = true,
        //            StatusCode = 200,
        //            StatusMessage = "Coupon applied successfully",
        //            Data = new RedeemCouponResponseDto
        //            {

        //            }
        //        });
        //    }
    }
}

