using AutoMapper;
using BlueBerry24.Services.UserCouponAPI.Models.DTOs;
using BlueBerry24.Services.UserCouponAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlueBerry24.Services.UserCouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCouponsController : ControllerBase
    {
        private readonly IUserCouponService _userCouponService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserCouponsController> _logger;

        public UserCouponsController(IUserCouponService userCouponService, IMapper mapper, ILogger<UserCouponsController> logger)
        {
            _userCouponService = userCouponService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        [Route("{userId:guid}")]
        public async Task<ActionResult<ResponseDto>> AddCouponToUser(string userId, [FromBody] UserCouponDto userCouponDto)
        {
            var entity = await _userCouponService.AddCouponToUserAsync(userId, userCouponDto.CouponId);


            if(entity != null)
            {
                return Ok(new ResponseDto
                {
                    Data = _mapper.Map<UserCouponDto>(entity),
                    IsSuccess = true,
                    StatusCode = 201
                });
            }

            return BadRequest(new ResponseDto
            {
                IsSuccess = false,
                StatusCode = 400,
                StatusMessage = "An error occurred while apply coupon to user"
            });
        }

        [HttpPut]
        [Route("disable-coupon/{userId}")]

        public async Task<ActionResult<ResponseDto>> DisableUserCoupon(string userId, UserCouponDto userCouponDto)
        {
            var isDisabled = await _userCouponService.DisableUserCouponAsync(userId, userCouponDto.CouponId);

            if (isDisabled)
            {
                return Ok(new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = 200
                });
            }

            return BadRequest(new ResponseDto
            {
                IsSuccess = false,
                StatusCode = 400,
                StatusMessage = "En error occurred while disabling the coupon"
            });
        }


        [HttpGet]
        [Route("coupons/{userId:guid}")]
        public async Task<ActionResult<ResponseDto>> GetCouponsByUserId(string userId)
        {
            var coupons = await _userCouponService.GetCouponsByUserIdAsync(userId);

            if(coupons.Count != 0)
            {
                return Ok(new ResponseDto
                {
                    Data = coupons,
                    IsSuccess = true,
                    StatusCode = 200
                });
            }

            return NotFound(new ResponseDto
            {
                IsSuccess = false,
                StatusCode = 404,
                StatusMessage = "There is no coupons found by user"
            });
        }

        [HttpGet]
        [Route("users/{couponId:guid}")]
        public async Task<ActionResult<ResponseDto>> GetUsersByCouponId(string couponId)
        {
            var users = await _userCouponService.GetUsersByCouponIdAsync(couponId);

            if(users.Count != 0)
            {
                return Ok(new ResponseDto
                {
                    Data = users,
                    IsSuccess = true,
                    StatusCode = 200
                });
            }

            return NotFound(new ResponseDto
            {
                IsSuccess = false,
                StatusCode = 404,
                StatusMessage = "There is no users found by coupon"
            });
        }


        [HttpGet]
        [Route("used-coupon/{userId:guid}")]
        public async Task<ActionResult<ResponseDto>> HasUserUsedCoupon(string userId, [FromQuery] string couponId)
        {

            var hasUsed = await _userCouponService.HasUserUsedCouponAsync(userId, couponId);

            if (hasUsed)
            {
                return Ok(new ResponseDto
                {
                    StatusCode = 200,
                    IsSuccess = true,
                    StatusMessage = "The coupon has used"
                });
            }


            return NotFound(new ResponseDto
            {
                StatusMessage = "The coupon has not used",
                StatusCode = 404,
                IsSuccess = false
            });
        }

    }
}
