
using BlueBerry24.Application.Dtos;
using BlueBerry24.Application.Dtos.CouponDtos;
using BlueBerry24.Application.Services.Interfaces.CouponServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BlueBerry24.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : BaseController
    {
        private readonly ICouponService _couponService;
        private readonly ILogger<CouponsController> _logger;

        public CouponsController(ICouponService couponService,
                                 ILogger<CouponsController> logger) : base(logger)
        {
            _couponService = couponService ?? throw new ArgumentNullException(nameof(couponService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetAll()
        {
            _logger.LogInformation("Getting all coupons");

            var coupons = await _couponService.GetAllAsync();

            if (coupons == null)
            {
                _logger.LogError("Error retrieving all coupons");
                return new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error retrieving coupons",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
            }
            var response = new ResponseDto
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Coupons retrieved successfully",
                Data = coupons
            };
            return Ok(response);


        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ResponseDto>> GetById(int id)
        {

            _logger.LogInformation($"Getting coupon with ID: {id}");
            var coupon = await _couponService.GetByIdAsync(id);

            if (coupon == null)
            {
                _logger.LogError($"Error retrieving coupon with ID {id}");
                return new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error retrieving coupon",
                    Errors = new List<string> { "An unexpected error occurred" }
                };

            }
            var response = new ResponseDto
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Coupon retrieved successfully",
                Data = coupon
            };
            return Ok(response);


        }

        [HttpGet]
        [Route("code/{code}")]
        public async Task<ActionResult<ResponseDto>> GetByCode(string code)
        {
            //if (string.IsNullOrWhiteSpace(code))
            //{
            //    var badRequestResponse = new ResponseDto
            //    {
            //        IsSuccess = false,
            //        StatusCode = StatusCodes.Status400BadRequest,
            //        StatusMessage = "Invalid request",
            //        Errors = new List<string> { "Coupon code cannot be empty" }
            //    };
            //    return BadRequest(badRequestResponse);
            //}


            _logger.LogInformation($"Getting coupon with code: {code}");
            var coupon = await _couponService.GetByCodeAsync(code);

            if (coupon == null)
            {
                _logger.LogError("Error retrieving coupon with code {code}");
                return new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error retrieving coupon",
                    Errors = new List<string> { "An unexpected error occurred" }
                };

            }
            var response = new ResponseDto
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Coupon retrieved successfully",
                Data = coupon
            };
            return Ok(response);






        }


        [HttpPost]
        public async Task<ActionResult<ResponseDto>> Create([FromBody] CreateCouponDto couponDto)
        {
            _logger.LogInformation($"Creating new coupon with code: {couponDto.Code}");
            var createdCoupon = await _couponService.CreateAsync(couponDto);

            if (createdCoupon == null)
            {
                _logger.LogError("Error creating coupon");
                return new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error creating coupon",
                    Errors = new List<string> { "An unexpected error occurred" }
                };

            }
            var response = new ResponseDto
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status201Created,
                StatusMessage = "Coupon created successfully",
                Data = createdCoupon
            };
            return response;


        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ResponseDto>> Update(int id, [FromBody] UpdateCouponDto couponDto)
        {
            //if (couponDto == null)
            //{
            //    var badRequestResponse = new ResponseDto
            //    {
            //        IsSuccess = false,
            //        StatusCode = StatusCodes.Status400BadRequest,
            //        StatusMessage = "Invalid request",
            //        Errors = new List<string> { "Coupon data is required" }
            //    };
            //    return BadRequest(badRequestResponse);
            //}


            _logger.LogInformation($"Updating coupon with ID: {id}");
            var updatedCoupon = await _couponService.UpdateAsync(id, couponDto);

            if (updatedCoupon == null)
            {
                _logger.LogError($"Error updating coupon with ID {id}");
                return new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error updating coupon",
                    Errors = new List<string> { "An unexpected error occurred" }
                };

            }
            var response = new ResponseDto
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Coupon updated successfully",
                Data = updatedCoupon
            };
            return Ok(response);
        }






        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ResponseDto>> Delete(int id)
        {

            _logger.LogInformation($"Deleting coupon with ID: {id}");
            var deleted = await _couponService.DeleteAsync(id);

            if (!deleted)
            {
                _logger.LogWarning($"Coupon with ID {id} not found for deletion");
                var notFoundResponse = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    StatusMessage = "Coupon not found",
                    Errors = new List<string> { $"Coupon with ID {id} not found" }
                };
                return NotFound(notFoundResponse);
            }

            var response = new ResponseDto
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Coupon deleted successfully"
            };
            return Ok(response);



        }

        [HttpGet]
        [Route("exists/{id}")]
        public async Task<ActionResult<ResponseDto>> Exists(int id)
        {

            var exists = await _couponService.ExistsByIdAsync(id);

            if (exists)
            {
                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Coupon exists",
                };
                return Ok(response);
            }

            var notFoundResponse = new ResponseDto
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status404NotFound,
                StatusMessage = "Coupon not found",
            };
            return NotFound(notFoundResponse);

        }

        //[HttpHead]
        //[Route("exists/{id:guid}")]
        //public async Task<IActionResult> ExistsById(string id)
        //{
        //    var exists = await _couponService.ExistsByIdAsync(id);

        //    return (!exists) ? NotFound() : Ok();
        //}

        [HttpGet]
        [Route("exists/code/{code}")]
        public async Task<ActionResult<ResponseDto>> ExistsByCode(string code)
        {
            //if (string.IsNullOrWhiteSpace(code))
            //{
            //    var badRequestResponse = new ResponseDto
            //    {
            //        IsSuccess = false,
            //        StatusCode = StatusCodes.Status400BadRequest,
            //        StatusMessage = "Invalid request",
            //        Errors = new List<string> { "Coupon code cannot be empty" }
            //    };
            //    return BadRequest(badRequestResponse);
            //}


            var exists = await _couponService.ExistsByCodeAsync(code);

            if (exists)
            {
                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Coupon exists",
                };
                return Ok(response);
            }

            var notFoundResponse = new ResponseDto
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status404NotFound,
                StatusMessage = "Coupon not found",
            };
            return NotFound(notFoundResponse);
        }

    }
}
