using AutoMapper;
using BlueBerry24.Services.ShoppingCartAPI.Data;
using BlueBerry24.Services.ShoppingCartAPI.Models;
using BlueBerry24.Services.ShoppingCartAPI.Models.DTOs;
using BlueBerry24.Services.ShoppingCartAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlueBerry24.Services.ShoppingCartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartsController : ControllerBase
    {
        private readonly ILogger<ShoppingCartsController> _logger;
        private readonly IMapper _mapper;
        private readonly ICartService _cartService;
        private readonly string? _userId;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShoppingCartsController(ILogger<ShoppingCartsController> logger, 
                                       IMapper mapper,
                                       ICartService cartService,
                                       IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _mapper = mapper;
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
            _userId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }


        [HttpGet]
        [Route("{userId}")]
        public async Task<ActionResult<ResponseDto>> GetCurrentUserCart(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("userId parameter is null or empty!", nameof(userId));
            }

            if(_userId != userId)
            {
                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "The requested is is not match with the current authenticated user."
                });
            }

            var cart = await _cartService.GetCartByUserIdAsync(userId);

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


        [HttpPut]
        [Route("redeem-coupon/{couponCode}")]
        public async Task<ActionResult<ResponseDto>> RedeemCoupon(string couponCode, RedeemCouponRequestDto redeemCouponDto)
        {
            var cart = await _cartService.ExistsByHeaderIdAsync(redeemCouponDto.UserId, redeemCouponDto.HeaderId);

            if (cart == null)
            {
                return NotFound(new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    StatusMessage = "The cart is not found"
                });
            }

            var redeemCoupon = await _cartService.RedeemCouponAsync(redeemCouponDto.UserId, redeemCouponDto.HeaderId, couponCode, redeemCouponDto.total);


            if(redeemCoupon == redeemCouponDto.total)
            {
                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "The total price doesn't match the minimum required for coupon"
                });
            }
            else
            {

                return Ok(new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "Coupon applied successfully",
                    Data = new RedeemCouponResponseDto
                    {
                        discountedTotal = redeemCoupon
                    }
                });
            }
        }
    }
}
