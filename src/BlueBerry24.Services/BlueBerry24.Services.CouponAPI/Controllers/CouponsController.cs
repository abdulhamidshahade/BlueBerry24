using BlueBerry24.Services.CouponAPI.Exceptions;
using BlueBerry24.Services.CouponAPI.Models.DTOs;
using BlueBerry24.Services.CouponAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BlueBerry24.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
        private readonly ICouponService _couponService;
        private readonly ILogger<CouponsController> _logger;

        public CouponsController(ICouponService couponService, ILogger<CouponsController> logger)
        {
            _couponService = couponService ?? throw new ArgumentNullException(nameof(couponService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetAll()
        {
            _logger.LogInformation("Getting all coupons");
            try
            {
                var coupons = await _couponService.GetAllAsync();
                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Coupons retrieved successfully",
                    Data = coupons
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all coupons");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error retrieving coupons",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        [HttpGet("{id:guid}", Name = "GetCouponById")]
        public async Task<ActionResult<ResponseDto>> GetById(string id)
        {
            try
            {
                _logger.LogInformation($"Getting coupon with ID: {id}");
                var coupon = await _couponService.GetByIdAsync(id);
                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Coupon retrieved successfully",
                    Data = coupon
                };
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, $"Coupon with ID {id} not found");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    StatusMessage = "Coupon not found",
                    Errors = new List<string> { ex.Message }
                };
                return NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving coupon with ID {id}");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error retrieving coupon",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("code/{code}", Name = "GetCouponByCode")]
        public async Task<ActionResult<ResponseDto>> GetByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                var badRequestResponse = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = "Invalid request",
                    Errors = new List<string> { "Coupon code cannot be empty" }
                };
                return BadRequest(badRequestResponse);
            }

            try
            {
                _logger.LogInformation($"Getting coupon with code: {code}");
                var coupon = await _couponService.GetByCodeAsync(code);
                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Coupon retrieved successfully",
                    Data = coupon
                };
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, $"Coupon with code {code} not found");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    StatusMessage = "Coupon not found",
                    Errors = new List<string> { ex.Message }
                };
                return NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving coupon with code {code}");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error retrieving coupon",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        [HttpPost]
        public async Task<ActionResult<ResponseDto>> Create([FromBody] CouponDto couponDto)
        {
            if (couponDto == null)
            {
                var badRequestResponse = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = "Invalid request",
                    Errors = new List<string> { "Coupon data is required" }
                };
                return BadRequest(badRequestResponse);
            }

            try
            {
                _logger.LogInformation($"Creating new coupon with code: {couponDto.Code}");
                var createdCoupon = await _couponService.CreateAsync(couponDto);
                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status201Created,
                    StatusMessage = "Coupon created successfully",
                    Data = createdCoupon
                };
                return CreatedAtRoute("GetCouponById", new { id = createdCoupon.Id }, response);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation failed when creating coupon");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = "Validation failed",
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(response);
            }
            catch (DuplicateEntityException ex)
            {
                _logger.LogWarning(ex, $"Coupon with code {couponDto.Code} already exists");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    StatusMessage = "Coupon already exists",
                    Errors = new List<string> { ex.Message }
                };
                return Conflict(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating coupon");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error creating coupon",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ResponseDto>> Update(string id, [FromBody] CouponDto couponDto)
        {
            if (couponDto == null)
            {
                var badRequestResponse = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = "Invalid request",
                    Errors = new List<string> { "Coupon data is required" }
                };
                return BadRequest(badRequestResponse);
            }

            try
            {
                _logger.LogInformation($"Updating coupon with ID: {id}");
                var updatedCoupon = await _couponService.UpdateAsync(id, couponDto);
                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Coupon updated successfully",
                    Data = updatedCoupon
                };
                return Ok(response);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation failed when updating coupon");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = "Validation failed",
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(response);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, $"Coupon with ID {id} not found");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    StatusMessage = "Coupon not found",
                    Errors = new List<string> { ex.Message }
                };
                return NotFound(response);
            }
            catch (DuplicateEntityException ex)
            {
                _logger.LogWarning(ex, $"Coupon with code {couponDto.Code} already exists");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    StatusMessage = "Coupon already exists",
                    Errors = new List<string> { ex.Message }
                };
                return Conflict(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating coupon with ID {id}");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error updating coupon",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ResponseDto>> Delete(string id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting coupon with ID {id}");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error deleting coupon",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpHead("{id:guid}")]
        [HttpGet("exists/{id:int}")]
        public async Task<ActionResult<ResponseDto>> Exists(string id)
        {
            try
            {
                var exists = await _couponService.ExistsAsync(id);

                if (exists)
                {
                    var response = new ResponseDto
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK,
                        StatusMessage = "Coupon exists",
                        Data = true
                    };
                    return Ok(response);
                }

                var notFoundResponse = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    StatusMessage = "Coupon not found",
                    Data = false
                };
                return NotFound(notFoundResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking existence of coupon with ID {id}");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error checking coupon existence",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpHead("code/{code}")]
        [HttpGet("exists/code/{code}")]
        public async Task<ActionResult<ResponseDto>> ExistsByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                var badRequestResponse = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = "Invalid request",
                    Errors = new List<string> { "Coupon code cannot be empty" }
                };
                return BadRequest(badRequestResponse);
            }

            try
            {
                var exists = await _couponService.ExistsByCodeAsync(code);

                if (exists)
                {
                    var response = new ResponseDto
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK,
                        StatusMessage = "Coupon exists",
                        Data = true
                    };
                    return Ok(response);
                }

                var notFoundResponse = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    StatusMessage = "Coupon not found",
                    Data = false
                };
                return NotFound(notFoundResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking existence of coupon with code {code}");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error checking coupon existence",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
