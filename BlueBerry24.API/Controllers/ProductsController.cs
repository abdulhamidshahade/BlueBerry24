using BlueBerry24.Application.Authorization.Attributes;
using BlueBerry24.Application.Dtos;
using BlueBerry24.Application.Dtos.ProductDtos;
using BlueBerry24.Application.Services.Interfaces.ProductServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlueBerry24.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;


        public ProductsController(IProductService productService,
                                  ILogger<ProductsController> logger) : base(logger)
        {
            _productService = productService;
            _logger = logger;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto>> GetAll()
        {
            _logger.LogInformation("Getting all products");

            var products = await _productService.GetAllAsync();

            if (products == null)
            {
                _logger.LogError("Error retrieving all products");
                return new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error retrieving products",
                    Errors = new List<string> { "An unexpected error occurred" }
                };

            }
            var response = new ResponseDto
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Products retrieved successfully",
                Data = products
            };
            return Ok(response);

        }

        [HttpGet]
        [Route("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto>> GetById(int id)
        {
            _logger.LogInformation($"Getting product with ID: {id}");
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                _logger.LogError($"Error retrieving product with ID {id}");
                return new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error retrieving product",
                    Errors = new List<string> { "An unexpected error occurred" }
                };

            }
            var response = new ResponseDto
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Product retrieved successfully",
                Data = product
            };
            return Ok(response);


        }

        [HttpGet]
        [Route("name/{name}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto>> GetByName(string name)
        {
            _logger.LogInformation($"Getting product with name: {name}");
            var product = await _productService.GetByNameAsync(name);

            if (product == null)
            {
                _logger.LogError($"Error retrieving product with name {name}");
                return new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error retrieving product",
                    Errors = new List<string> { "An unexpected error occurred" }
                };

            }
            var response = new ResponseDto
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Product retrieved successfully",
                Data = product
            };
            return Ok(response);
        }

        [HttpPost]
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto>> Create([FromBody] CreateProductDto productDto,
            [FromQuery] List<int> categories)
        {
            _logger.LogInformation($"Creating new product with name: {productDto.Name}");
            var createdProduct = await _productService.CreateAsync(productDto, categories);

            if (createdProduct == null)
            {
                _logger.LogError("Error creating product");
                return new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error creating product",
                    Errors = new List<string> { "An unexpected error occurred" }
                };

            }

            var response = new ResponseDto
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status201Created,
                StatusMessage = "Product created successfully",
                Data = createdProduct
            };

            return response;
        }

        [HttpPut]
        [Route("{id}")]
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto>> Update(int id, [FromBody] UpdateProductDto productDto,
            [FromQuery] List<int> categories)
        {
            _logger.LogInformation($"Updating product with Id: {id}");
            var updatedProduct = await _productService.UpdateAsync(id, productDto, categories);

            if (updatedProduct == null)
            {
                _logger.LogError($"Error updating product with ID {id}");
                return new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error updating product",
                    Errors = new List<string> { "An unexpected error occurred" }
                };

            }

            var response = new ResponseDto
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Product updated successfully",
                Data = updatedProduct
            };
            return Ok(response);
        }


        [HttpDelete]
        [Route("{id}")]
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto>> Delete(int id)
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
                    Errors = new List<string> { $"Product with ID {id} not found" },
                    Data = false
                };
                return NotFound(notFoundResponse);
            }

            var response = new ResponseDto
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Product deleted successfully",
                Data = true
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("exists/{id}")]
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto>> ExistsById(int id)
        {

            var exists = await _productService.ExistsByIdAsync(id);

            if (exists)
            {
                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Product exists",
                    Data = true
                };
                return Ok(response);
            }

            var notFoundResponse = new ResponseDto
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status404NotFound,
                StatusMessage = "Product not found",
                Data = false
            };
            return NotFound(notFoundResponse);
        }

        [HttpGet]
        [Route("exists/name/{name}")]
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto>> ExistsByName(string name)
        {
            var exists = await _productService.ExistsByNameAsync(name);

            if (exists)
            {
                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Product exists",
                    Data = true
                };
                return Ok(response);
            }

            var notFoundResponse = new ResponseDto
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status404NotFound,
                StatusMessage = "Product not found",
                Data = false
            };

            return NotFound(notFoundResponse);
        }
    }
}
