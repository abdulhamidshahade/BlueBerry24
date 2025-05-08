using BlueBerry24.Application.Dtos;
using BlueBerry24.Application.Dtos.ShopDtos;
using BlueBerry24.Application.Services.Interfaces.ShopServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlueBerry24.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        private readonly IShopService _shopService;

        private readonly ILogger<ShopsController> _logger;

        public ShopsController(IShopService shopService,
                               ILogger<ShopsController> logger)
        {
            _shopService = shopService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetAll()
        {
            _logger.LogInformation("Getting all shops");

            var shops = await _shopService.GetAllAsync();

            if (shops == null)
            {
                _logger.LogError("Error retrieving all shops");
                return new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error retrieving shops",
                    Errors = new List<string> { "An unexpected error occurred" }
                };

            }
            var response = new ResponseDto
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Shops retrieved successfully",
                Data = shops
            };
            return Ok(response);


        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ResponseDto>> GetById(int id)
        {

            _logger.LogInformation($"Getting shop with ID: {id}");
            var shop = await _shopService.GetByIdAsync(id);

            if (shop == null)
            {
                _logger.LogError($"Error retrieving shop with ID {id}");
                return new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error retrieving shop",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
            }
            var response = new ResponseDto
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Shop retrieved successfully",
                Data = shop
            };
            return Ok(response);


        }

        [HttpGet]
        [Route("name/{name}")]
        public async Task<ActionResult<ResponseDto>> GetByName(string name)
        {
            //if (string.IsNullOrWhiteSpace(name))
            //{
            //    var badRequestResponse = new ResponseDto
            //    {
            //        IsSuccess = false,
            //        StatusCode = StatusCodes.Status400BadRequest,
            //        StatusMessage = "Invalid request",
            //        Errors = new List<string> { "Shop name cannot be empty" }
            //    };
            //    return BadRequest(badRequestResponse);
            //}


            _logger.LogInformation($"Getting shop with name: {name}");
            var shop = await _shopService.GetByNameAsync(name);

            if (shop == null)
            {
                _logger.LogError($"Error retrieving shop with name {name}");
                return new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error retrieving shop",
                    Errors = new List<string> { "An unexpected error occurred" }
                };

            }
            var response = new ResponseDto
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Shop retrieved successfully",
                Data = shop
            };
            return Ok(response);


        }


        [HttpPost]
        public async Task<ActionResult<ResponseDto>> Create([FromBody] CreateShopDto shopDto)
        {
            //if (shopDto == null)
            //{
            //    var badRequestResponse = new ResponseDto
            //    {
            //        IsSuccess = false,
            //        StatusCode = StatusCodes.Status400BadRequest,
            //        StatusMessage = "Invalid request",
            //        Errors = new List<string> { "Shop data is required" }
            //    };
            //    return BadRequest(badRequestResponse);
            //}


            _logger.LogInformation($"Creating new shop with name: {shopDto.Name}");
            var createdShop = await _shopService.CreateAsync(shopDto);

            if (createdShop == null)
            {
                _logger.LogError("Error creating shop");
                return new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error creating shop",
                    Errors = new List<string> { "An unexpected error occurred" }
                };

            }
            var response = new ResponseDto
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status201Created,
                StatusMessage = "Coupon created successfully",
                Data = createdShop
            };
            return Ok(response);


        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ResponseDto>> Update(int id, [FromBody] UpdateShopDto shopDto)
        {
            //if (shopDto == null)
            //{
            //    var badRequestResponse = new ResponseDto
            //    {
            //        IsSuccess = false,
            //        StatusCode = StatusCodes.Status400BadRequest,
            //        StatusMessage = "Invalid request",
            //        Errors = new List<string> { "Shop data is required" }
            //    };
            //    return BadRequest(badRequestResponse);
            //}


            _logger.LogInformation($"Updating shop with ID: {id}");
            var updatedShop = await _shopService.UpdateAsync(id, shopDto);

            if (updatedShop == null)
            {
                _logger.LogError($"Error updating shop with ID {id}");
                return new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error updating shop",
                    Errors = new List<string> { "An unexpected error occurred" }
                };

            }
            var response = new ResponseDto
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Shop updated successfully",
                Data = updatedShop
            };
            return Ok(response);

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ResponseDto>> Delete(int id)
        {

            _logger.LogInformation($"Deleting coupon with ID: {id}");
            var deleted = await _shopService.DeleteAsync(id);


            if (!deleted)
            {
                _logger.LogError($"Error deleting shop with ID {id}");
                return new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error deleting shop",
                    Errors = new List<string> { "An unexpected error occurred" }
                };

            }

            var response = new ResponseDto
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Shop deleted successfully"
            };
            return Ok(response);
        }





        [HttpGet]
        [Route("exists/{id}")]
        public async Task<ActionResult<ResponseDto>> ExistsById(int id)
        {

            var exists = await _shopService.ExistsByIdAsync(id);

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

            _logger.LogError($"Error checking existence of shop with ID {id}");
            return new ResponseDto
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
                StatusMessage = "Error checking shop existence",
                Errors = new List<string> { "An unexpected error occurred" }
            };
        }





        [HttpGet]
        [Route("exists/name/{name}")]
        public async Task<ActionResult<ResponseDto>> ExistsByName(string name)
        {
            //if (string.IsNullOrWhiteSpace(name))
            //{
            //    var badRequestResponse = new ResponseDto
            //    {
            //        IsSuccess = false,
            //        StatusCode = StatusCodes.Status400BadRequest,
            //        StatusMessage = "Invalid request",
            //        Errors = new List<string> { "Shop name cannot be empty" }
            //    };
            //    return BadRequest(badRequestResponse);
            //}


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

            _logger.LogError($"Error checking existence of shop with name {name}");
            return new ResponseDto
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
                StatusMessage = "Error checking shop existence",
                Errors = new List<string> { "An unexpected error occurred" }
            };


        }

    }
}
