using BlueBerry24.Application.Dtos;
using BlueBerry24.Application.Dtos.CouponDtos;
using BlueBerry24.Application.Services.Interfaces.CouponServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlueBerry24.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCouponsController : BaseController
    {
        private readonly IUserCouponService _userCouponService;
        private readonly ILogger<UserCouponsController> _logger;

        public UserCouponsController(IUserCouponService userCouponService,
                                     ILogger<UserCouponsController> logger) : base(logger)
        {
            _userCouponService = userCouponService;
            _logger = logger;
        }

        [HttpPost]
        [Route("{userId}")]
        public async Task<ActionResult<ResponseDto>> AddCouponToUser(int userId, [FromBody] UserCouponDto userCouponDto)
        {
            var entity = await _userCouponService.AddCouponToUserAsync(userId, userCouponDto.CouponId);


            if(entity != null)
            {
                return Ok(new ResponseDto
                {
                    Data = entity,
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

        public async Task<ActionResult<ResponseDto>> DisableUserCoupon(int userId, UserCouponDto userCouponDto)
        {
            var isDisabled = await _userCouponService.DisableCouponToUser(userId, userCouponDto.CouponId);

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
        [Route("coupons/{userId}")]
        public async Task<ActionResult<ResponseDto>> GetCouponsByUserId(int userId)
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
        [Route("users/{couponId}")]
        public async Task<ActionResult<ResponseDto>> GetUsersByCouponId(int couponId)
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
        [Route("used-coupon/{couponCode}")]
        public async Task<ActionResult<ResponseDto>> HasUserUsedCoupon(int userId, string couponCode)
        {

            var hasUsed = await _userCouponService.IsCouponUsedByUser(userId, couponCode);

            if (hasUsed)
            {
                return Ok(new ResponseDto
                {
                    StatusCode = 200,
                    IsSuccess = true,
                    StatusMessage = "The coupon has used"
                });
            }
            else if (!hasUsed)
            {
                return Ok(new ResponseDto
                {
                    StatusMessage = "The coupon has not used",
                    StatusCode = 200,
                    IsSuccess = true
                });
            }

            else
            {
                return BadRequest(new ResponseDto
                {
                    StatusMessage = "Error exists while checking",
                    StatusCode = 400,
                    IsSuccess = false
                });
            }
        }

    }
}
