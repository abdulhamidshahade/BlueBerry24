using BlueBerry24.Application.Dtos;
using BlueBerry24.Application.Dtos.CategoryDtos;
using BlueBerry24.Application.Services.Interfaces.ProductServiceInterfaces;

using Microsoft.AspNetCore.Mvc;

namespace BlueBerry24.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _categoryService;


        private readonly ILogger<CategoriesController> _logger;
        public CategoriesController(ILogger<CategoriesController> logger,
                                    ICategoryService categoryService) : base(logger)
        {
            _logger = logger;
            _categoryService = categoryService;
        }


        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetAll()
        {
            _logger.LogInformation("Getting all categories");

            var categories = await _categoryService.GetAllAsync();



            if (categories == null)
            {
                return StatusCode(400, new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = "Failed retriving categories"
                });
            }

            var response = new ResponseDto
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Categories retrieved successfully",
                Data = categories
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ResponseDto>> GetById(int id)
        {

            _logger.LogInformation($"Getting category with ID: {id}");
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
            {
                _logger.LogWarning($"Category with ID {id} not found");
                return NotFound(new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    StatusMessage = "Category not found",
                });
            }

            var response = new ResponseDto
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Category retrieved successfully",
                Data = category
            };

            return Ok(response);

        }


        [HttpGet]
        [Route("name/{name}")]
        public async Task<ActionResult<ResponseDto>> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                var badRequestResponse = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = "Invalid request",
                    Errors = new List<string> { "Category name cannot be empty" }
                };
                return BadRequest(badRequestResponse);
            }


            _logger.LogInformation($"Getting category with name: {name}");
            var product = await _categoryService.GetByNameAsync(name);

            if (product == null)
            {
                _logger.LogWarning($"Category with name {name} not found");
                return NotFound(new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    StatusMessage = "Category not found",
                });
            }

            var response = new ResponseDto
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Category retrieved successfully",
                Data = product
            };
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto>> Create([FromBody] CreateCategoryDto categoryDto)
        {
            _logger.LogInformation($"Creating new category with name: {categoryDto.Name}");
            var createdCategory = await _categoryService.CreateAsync(categoryDto);

            if (createdCategory == null)
            {
                _logger.LogError("Error creating category");
                return new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error creating category",
                    Errors = new List<string> { "An unexpected error occurred" }
                };

            }
            var response = new ResponseDto
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status201Created,
                StatusMessage = "Category created successfully",
                Data = createdCategory
            };
            return response;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ResponseDto>> Update(int id, [FromBody] UpdateCategoryDto categoryDto)
        {
            _logger.LogInformation($"Updating category with Id: {id}");
            var updatedCategory = await _categoryService.UpdateAsync(id, categoryDto);

            if (updatedCategory == null)
            {
                _logger.LogError($"Error updating category with ID {id}");
                return new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error updating category",
                    Errors = new List<string> { "An unexpected error occurred" }
                };

            }

            var response = new ResponseDto
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Category updated successfully",
                Data = updatedCategory
            };
            return Ok(response);

        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ResponseDto>> Delete(int id)
        {

            _logger.LogInformation($"Deleting category with ID: {id}");
            var deleted = await _categoryService.DeleteAsync(id);

            if (!deleted)
            {
                _logger.LogWarning($"Category with ID {id} not found for deletion");
                var notFoundResponse = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    StatusMessage = "Category not found",
                    Errors = new List<string> { $"Category with ID {id} not found" }
                };
                return NotFound(notFoundResponse);
            }

            var response = new ResponseDto
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Category deleted successfully"
            };
            return Ok(response);

        }

        [HttpGet]
        [Route("exists/{id}")]
        public async Task<ActionResult<ResponseDto>> ExistsById(int id)
        {

            var exists = await _categoryService.ExistsAsync(id);

            if (exists)
            {
                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Category exists",
                    Data = true
                };
                return Ok(response);
            }

            var notFoundResponse = new ResponseDto
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status404NotFound,
                StatusMessage = "Category not found",
                Data = false
            };
            return NotFound(notFoundResponse);


        }


        [HttpGet]
        [Route("exists/name/{name}")]
        public async Task<ActionResult<ResponseDto>> ExistsByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                var badRequestResponse = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = "Invalid request",
                    Errors = new List<string> { "Category name cannot be empty" }
                };
                return BadRequest(badRequestResponse);
            }


            var exists = await _categoryService.ExistsByNameAsync(name);

            if (exists)
            {
                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Category exists",
                    Data = true
                };
                return Ok(response);
            }

            var notFoundResponse = new ResponseDto
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status404NotFound,
                StatusMessage = "Category not found",
                Data = false
            };
            return NotFound(notFoundResponse);
        }


    }

}
