using Berryfy.Application.Authorization.Attributes;
using Berryfy.Application.Dtos;
using Berryfy.Application.Dtos.CheckoutDtos;
using Berryfy.Application.Services.Interfaces.CheckoutServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Berryfy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserCheckoutInfoController : BaseController
    {
        private readonly IUserCheckoutInfoService _checkoutInfoService;

        public UserCheckoutInfoController(IUserCheckoutInfoService checkoutInfoService)
        {
            _checkoutInfoService = checkoutInfoService;
        }

        [HttpGet]
        [UserAndAbove]
        public async Task<ActionResult<ResponseDto<UserCheckoutInfoDto?>>> Get()
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return Unauthorized(new ResponseDto<UserCheckoutInfoDto?>
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    StatusMessage = "Authentication required"
                });
            }

            var info = await _checkoutInfoService.GetCheckoutInfoAsync(userId.Value);

            return Ok(new ResponseDto<UserCheckoutInfoDto?>
            {
                IsSuccess = true,
                StatusCode = 200,
                StatusMessage = info == null ? "No saved checkout information" : "Checkout information retrieved",
                Data = info
            });
        }

        [HttpPost("checkout")]
        [UserAndAbove]
        public async Task<ActionResult<ResponseDto<UserCheckoutInfoDto>>> SaveCheckout([FromBody] SaveCheckoutInfoDto? dto)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return Unauthorized(new ResponseDto<UserCheckoutInfoDto>
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    StatusMessage = "Authentication required"
                });
            }

            if (dto == null)
            {
                return BadRequest(new ResponseDto<UserCheckoutInfoDto>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Request body is required"
                });
            }

            var saved = await _checkoutInfoService.SaveCheckoutInfoAsync(userId.Value, dto);

            return Ok(new ResponseDto<UserCheckoutInfoDto>
            {
                IsSuccess = true,
                StatusCode = 200,
                StatusMessage = "Checkout information saved",
                Data = saved
            });
        }

        [HttpPost("billing")]
        [UserAndAbove]
        public async Task<ActionResult<ResponseDto<UserCheckoutInfoDto>>> SaveBilling([FromBody] SavePaymentBillingDto? dto)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return Unauthorized(new ResponseDto<UserCheckoutInfoDto>
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    StatusMessage = "Authentication required"
                });
            }

            if (dto == null)
            {
                return BadRequest(new ResponseDto<UserCheckoutInfoDto>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Request body is required"
                });
            }

            var saved = await _checkoutInfoService.SavePaymentBillingInfoAsync(userId.Value, dto);

            return Ok(new ResponseDto<UserCheckoutInfoDto>
            {
                IsSuccess = true,
                StatusCode = 200,
                StatusMessage = "Billing information saved",
                Data = saved
            });
        }

        [HttpDelete]
        [UserAndAbove]
        public async Task<ActionResult<ResponseDto<object?>>> Delete()
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return Unauthorized(new ResponseDto<object?>
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    StatusMessage = "Authentication required"
                });
            }

            await _checkoutInfoService.DeleteCheckoutInfoAsync(userId.Value);

            return Ok(new ResponseDto<object?>
            {
                IsSuccess = true,
                StatusCode = 200,
                StatusMessage = "Checkout information removed",
                Data = null
            });
        }
    }
}
