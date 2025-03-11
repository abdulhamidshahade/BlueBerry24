using AutoMapper;
using BlueBerry24.Services.CouponAPI.Models.DTOs;
using BlueBerry24.Services.ProductAPI.Exceptions;
using BlueBerry24.Services.ProductAPI.Models.DTOs.ProductDtos;
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


        [HttpGet("name/{name}", Name = "GetProductByName")]
        public async Task<ActionResult<ResponseDto>> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                var badRequestResponse = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = "Invalid request",
                    Errors = new List<string> { "Product name cannot be empty" }
                };
                return BadRequest(badRequestResponse);
            }

            try
            {
                _logger.LogInformation($"Getting product with name: {name}");
                var product = await _productService.GetByNameAsync(name);
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
                _logger.LogWarning(ex, $"Product with name {name} not found");
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
                _logger.LogError(ex, $"Error retrieving product with name {name}");
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

        [HttpPost]
        public async Task<ActionResult<ResponseDto>> Create([FromBody] CreateProductDto productDto)
        {
            if (productDto == null)
            {
                var badRequestResponse = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = "Invalid request",
                    Errors = new List<string> { "Product data is required" }
                };
                return BadRequest(badRequestResponse);
            }

            try
            {
                _logger.LogInformation($"Creating new product with name: {productDto.Name}");
                var createdProduct = await _productService.CreateAsync(productDto);
                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status201Created,
                    StatusMessage = "Product created successfully",
                    Data = createdProduct
                };
                return CreatedAtRoute("GetProductById", new { id = createdProduct.Id }, response);
            }
            catch (ValidateException ex)
            {
                _logger.LogWarning(ex, "Validation failed when creating product");
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
                _logger.LogWarning(ex, $"Product with name {productDto.Name} already exists");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    StatusMessage = "Product already exists",
                    Errors = new List<string> { ex.Message }
                };
                return Conflict(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error creating product",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<ResponseDto>> Update(int id, [FromBody] UpdateProductDto productDto)
        {
            if (productDto == null)
            {
                var badRequestResponse = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = "Invalid request",
                    Errors = new List<string> { "Product data is required" }
                };
                return BadRequest(badRequestResponse);
            }

            try
            {
                _logger.LogInformation($"Updating product with Id: {id}");
                var updatedCoupon = await _productService.UpdateAsync(id, productDto);
                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Product updated successfully",
                    Data = updatedCoupon
                };
                return Ok(response);
            }
            catch (ValidateException ex)
            {
                _logger.LogWarning(ex, "Validation failed when updating product");
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
            catch (DuplicateEntityException ex)
            {
                _logger.LogWarning(ex, $"Product with name {productDto.Name} already exists");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    StatusMessage = "Product already exists",
                    Errors = new List<string> { ex.Message }
                };
                return Conflict(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating product with ID {id}");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error updating product",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ResponseDto>> Delete(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting product with ID: {id}");
                var deleted = await _productService.DeleteAsync(id);

                if (!deleted)
                {
                    _logger.LogWarning($"Product with ID {id} not found for deletion");
                    var notFoundResponse = new ResponseDto
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status404NotFound,
                        StatusMessage = "Product not found",
                        Errors = new List<string> { $"Product with ID {id} not found" }
                    };
                    return NotFound(notFoundResponse);
                }

                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Product deleted successfully"
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting product with ID {id}");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error deleting product",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
