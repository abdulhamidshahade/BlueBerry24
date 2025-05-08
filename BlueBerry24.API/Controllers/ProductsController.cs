using AutoMapper;
using BlueBerry24.Application.Dtos;
using BlueBerry24.Application.Dtos.ProductDtos;
using BlueBerry24.Application.Services.Interfaces.ProductServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlueBerry24.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService,
                                  ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }


        [HttpGet]
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
        public async Task<ActionResult<ResponseDto>> GetById(int id)
        {
            
                _logger.LogInformation($"Getting product with ID: {id}");
                var product = await _productService.GetByIdAsync(id);

                if(product == null)
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
        public async Task<ActionResult<ResponseDto>> GetByName(string name)
        {
            //if (string.IsNullOrWhiteSpace(name))
            //{
            //    var badRequestResponse = new ResponseDto
            //    {
            //        IsSuccess = false,
            //        StatusCode = StatusCodes.Status400BadRequest,
            //        StatusMessage = "Invalid request",
            //        Errors = new List<string> { "Product name cannot be empty" }
            //    };
            //    return BadRequest(badRequestResponse);
            //}


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
        public async Task<ActionResult<ResponseDto>> Create([FromBody] CreateProductDto productDto,
            [FromQuery] List<int> categories)
        {
            //if (productDto == null)
            //{
            //    var badRequestResponse = new ResponseDto
            //    {
            //        IsSuccess = false,
            //        StatusCode = StatusCodes.Status400BadRequest,
            //        StatusMessage = "Invalid request",
            //        Errors = new List<string> { "Product data is required" }
            //    };
            //    return BadRequest(badRequestResponse);
            //}


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

            return CreatedAtRoute("GetProductById", new { id = createdProduct.Id }, response);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ResponseDto>> Update(int id, [FromBody] UpdateProductDto productDto,
            [FromQuery] List<int> categories)
        {
            //if (productDto == null)
            //{
            //    var badRequestResponse = new ResponseDto
            //    {
            //        IsSuccess = false,
            //        StatusCode = StatusCodes.Status400BadRequest,
            //        StatusMessage = "Invalid request",
            //        Errors = new List<string> { "Product data is required" }
            //    };
            //    return BadRequest(badRequestResponse);
            //}


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

        [HttpGet]
        [Route("exists/{id}")]
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
        public async Task<ActionResult<ResponseDto>> ExistsByName(string name)
        {
            //if (string.IsNullOrWhiteSpace(name))
            //{
            //    var badRequestResponse = new ResponseDto
            //    {
            //        IsSuccess = false,
            //        StatusCode = StatusCodes.Status400BadRequest,
            //        StatusMessage = "Invalid request",
            //        Errors = new List<string> { "Product name cannot be empty" }
            //    };
            //    return BadRequest(badRequestResponse);
            //}


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




        //[HttpGet]
        //[Route("exists/shop/{productId}")]
        //public async Task<ActionResult<ResponseDto>> ExistsByShopId(string productId, [FromQuery] string shopId)
        //{
        //    if(string.IsNullOrEmpty(productId) || string.IsNullOrEmpty(shopId))
        //    {
        //        return BadRequest(new ResponseDto
        //        {
        //            IsSuccess = false,
        //            StatusCode = 400,
        //            StatusMessage = "product id or shop id is null"
        //        });
        //    }

        //    var exists = await _productService.ExistsByShopIdAsync(productId, shopId);

        //    if (exists)
        //    {
        //        return Ok(new ResponseDto
        //        {
        //            IsSuccess = true,
        //            StatusCode = 200,
        //            StatusMessage = "The product exists by shop successfully"
        //        });
        //    }
        //    return NotFound(new ResponseDto
        //    {
        //        IsSuccess = false,
        //        StatusCode = 404,
        //        StatusMessage = "The product don't found by shop id"
        //    });
        //}
    }
}
