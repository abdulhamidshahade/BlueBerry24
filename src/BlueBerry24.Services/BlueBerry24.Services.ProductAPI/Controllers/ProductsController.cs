using AutoMapper;
using BlueBerry24.Services.CouponAPI.Models.DTOs;
using BlueBerry24.Services.ProductAPI.Exceptions;
using BlueBerry24.Services.ProductAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlueBerry24.Services.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, IMapper mapper,
            ILogger<ProductsController> logger)
        {
            _productService = productService;
            _mapper = mapper;
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetAll()
        {
            _logger.LogInformation("Getting all products");
            try
            {
                var products = await _productService.GetAllAsync();
                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Products retrieved successfully",
                    Data = products
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all products");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error retrieving products",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("{id:int}", Name = "GetProductById")]
        public async Task<ActionResult<ResponseDto>> GetById(int id)
        {
            try
            {
                _logger.LogInformation($"Getting product with ID: {id}");
                var product = await _productService.GetByIdAsync(id);
                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Product retrieved successfully",
                    Data = product
                };
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, $"Product with ID {id} not found");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    StatusMessage = "Product not found",
                    Errors = new List<string> { ex.Message }
                };
                return NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving product with ID {id}");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error retrieving product",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

    }
}
