using BlueBerry24.Application.Dtos;
using BlueBerry24.Application.Dtos.ShopDtos;
using BlueBerry24.Application.Services.Interfaces.ShopServiceInterfaces;
using BlueBerry24.Domain.Entities.ShopEntities;
using Microsoft.AspNetCore.Mvc;

namespace BlueBerry24.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopsController : BaseController
    {
        private readonly IShopService _shopService;
        private readonly ILogger<ShopsController> _logger;

        public ShopsController(IShopService shopService,
                               ILogger<ShopsController> logger) : base(logger)
        {
            _shopService = shopService;
            _logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ResponseDto<Shop>>> GetById(int id)
        {

            _logger.LogInformation($"Getting shop with ID: {id}");
            var shop = await _shopService.GetShopAsync(id);

            if (shop == null)
            {
                _logger.LogError($"Error retrieving shop with ID {id}");
                return new ResponseDto<Shop>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error retrieving shop",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
            }
            var response = new ResponseDto<Shop>
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Shop retrieved successfully",
                Data = shop
            };
            return Ok(response);


        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ResponseDto<ShopDto>>> Update(int id, [FromBody] UpdateShopDto shopDto)
        {

            _logger.LogInformation($"Updating shop with ID: {id}");
            var updatedShop = await _shopService.UpdateShopAsync(id, shopDto);

            if (updatedShop == null)
            {
                _logger.LogError($"Error updating shop with ID {id}");
                return new ResponseDto<ShopDto>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error updating shop",
                    Errors = new List<string> { "An unexpected error occurred" }
                };

            }
            var response = new ResponseDto<ShopDto>
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Shop updated successfully",
                Data = updatedShop
            };
            return Ok(response);

        }

    }
}
