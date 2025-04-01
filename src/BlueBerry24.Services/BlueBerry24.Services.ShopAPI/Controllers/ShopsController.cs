using AutoMapper;
using BlueBerry24.Services.ShopAPI.Exceptions;
using BlueBerry24.Services.ShopAPI.Models.DTOs;
using BlueBerry24.Services.ShopAPI.Models.DTOs.ShopDtos;
using BlueBerry24.Services.ShopAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlueBerry24.Services.ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        private readonly IShopService _shopService;
        private readonly IMapper _mapper;
        private readonly ILogger<ShopsController> _logger;

        public ShopsController(IShopService shopService, IMapper mapper, ILogger<ShopsController> logger)
        {
            _shopService = shopService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetAll()
        {
            _logger.LogInformation("Getting all shops");
            try
            {
                var shops = await _shopService.GetAllAsync();
                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Shops retrieved successfully",
                    Data = shops
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all shops");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error retrieving shops",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ResponseDto>> GetById(string id)
        {
            try
            {
                _logger.LogInformation($"Getting shop with ID: {id}");
                var shop = await _shopService.GetByIdAsync(id);
                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Shop retrieved successfully",
                    Data = shop
                };
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, $"Shop with ID {id} not found");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    StatusMessage = "Shop not found",
                    Errors = new List<string> { ex.Message }
                };
                return NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving shop with ID {id}");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error retrieving shop",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("name/{name}", Name = "GetShopByName")]
        public async Task<ActionResult<ResponseDto>> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                var badRequestResponse = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = "Invalid request",
                    Errors = new List<string> { "Shop name cannot be empty" }
                };
                return BadRequest(badRequestResponse);
            }

            try
            {
                _logger.LogInformation($"Getting shop with name: {name}");
                var shop = await _shopService.GetByNameAsync(name);
                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Shop retrieved successfully",
                    Data = shop
                };
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, $"Shop with code {name} not found");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    StatusMessage = "Shop not found",
                    Errors = new List<string> { ex.Message }
                };
                return NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving shop with name {name}");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error retrieving shop",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        [HttpPost]
        public async Task<ActionResult<ResponseDto>> Create([FromBody] CreateShopDto shopDto)
        {
            if (shopDto == null)
            {
                var badRequestResponse = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = "Invalid request",
                    Errors = new List<string> { "Shop data is required" }
                };
                return BadRequest(badRequestResponse);
            }

            try
            {
                _logger.LogInformation($"Creating new shop with name: {shopDto.Name}");
                var createdShop = await _shopService.CreateAsync(shopDto);
                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status201Created,
                    StatusMessage = "Coupon created successfully",
                    Data = createdShop
                };
                return Ok(response);
            }
            catch (ValidateException ex)
            {
                _logger.LogWarning(ex, "Validation failed when creating shop");
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
                _logger.LogWarning(ex, $"Shop with code {shopDto.Name} already exists");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    StatusMessage = "Shop already exists",
                    Errors = new List<string> { ex.Message }
                };
                return Conflict(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating shop");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error creating shop",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ResponseDto>> Update(string id, [FromBody] UpdateShopDto shopDto)
        {
            if (shopDto == null)
            {
                var badRequestResponse = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = "Invalid request",
                    Errors = new List<string> { "Shop data is required" }
                };
                return BadRequest(badRequestResponse);
            }

            try
            {
                _logger.LogInformation($"Updating shop with ID: {id}");
                var updatedShop = await _shopService.UpdateAsync(id, shopDto);
                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Shop updated successfully",
                    Data = updatedShop
                };
                return Ok(response);
            }
            catch (ValidateException ex)
            {
                _logger.LogWarning(ex, "Validation failed when updating shop");
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
                    StatusMessage = "Shop not found",
                    Errors = new List<string> { ex.Message }
                };
                return NotFound(response);
            }
            catch (DuplicateEntityException ex)
            {
                _logger.LogWarning(ex, $"Shop with name {shopDto.Name} already exists");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    StatusMessage = "Shop already exists",
                    Errors = new List<string> { ex.Message }
                };
                return Conflict(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating shop with ID {id}");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error updating shop",
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
                var deleted = await _shopService.DeleteAsync(id);

                if (!deleted)
                {
                    _logger.LogWarning($"Shop with ID {id} not found for deletion");
                    var notFoundResponse = new ResponseDto
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status404NotFound,
                        StatusMessage = "Coupon not found",
                        Errors = new List<string> { $"Shop with ID {id} not found" }
                    };
                    return NotFound(notFoundResponse);
                }

                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Shop deleted successfully"
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting shop with ID {id}");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error deleting shop",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("exists/{id}")]
        public async Task<ActionResult<ResponseDto>> Exists(string id)
        {
            try
            {
                var exists = await _shopService.ExistsAsync(id);

                if (exists)
                {
                    var response = new ResponseDto
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK,
                        StatusMessage = "Shop exists",
                    };
                    return Ok(response);
                }

                var notFoundResponse = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    StatusMessage = "Shop not found",
                };
                return NotFound(notFoundResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking existence of shop with ID {id}");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error checking shop existence",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("exists/name/{name}")]
        public async Task<ActionResult<ResponseDto>> ExistsByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                var badRequestResponse = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = "Invalid request",
                    Errors = new List<string> { "Shop name cannot be empty" }
                };
                return BadRequest(badRequestResponse);
            }

            try
            {
                var exists = await _shopService.ExistsByNameAsync(name);

                if (exists)
                {
                    var response = new ResponseDto
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK,
                        StatusMessage = "Shop exists",
                        Data = true
                    };
                    return Ok(response);
                }

                var notFoundResponse = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    StatusMessage = "Shop not found",
                    Data = false
                };
                return NotFound(notFoundResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking existence of shop with name {name}");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error checking shop existence",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

    }
}
