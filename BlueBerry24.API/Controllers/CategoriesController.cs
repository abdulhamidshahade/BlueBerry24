using BlueBerry24.Application.Authorization.Attributes;
using BlueBerry24.Application.Dtos;
using BlueBerry24.Application.Dtos.CategoryDtos;
using BlueBerry24.Application.Services.Interfaces.ProductServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlueBerry24.API.Controllers
{
    [Route("api/categories")]
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
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<IEnumerable<CategoryDto>>>> GetAll()
        {
            _logger.LogInformation("Getting all categories");
            var categories = await _categoryService.GetAllAsync();

            if (categories == null)
            {
                return StatusCode(400, new ResponseDto<IEnumerable<CategoryDto>>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = "Failed retriving categories",
                    Data = null
                });
            }

            var response = new ResponseDto<IEnumerable<CategoryDto>>
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
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<CategoryDto>>> GetById(int id)
        {

            _logger.LogInformation($"Getting category with ID: {id}");
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
            {
                _logger.LogWarning($"Category with ID {id} not found");
                return NotFound(new ResponseDto<CategoryDto>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    StatusMessage = "Category not found",
                    Data = null
                });
            }

            var response = new ResponseDto<CategoryDto>
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
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<CategoryDto>>> GetByName(string name)
        {
            _logger.LogInformation($"Getting category with name: {name}");
            var category = await _categoryService.GetByNameAsync(name);

            if (category == null)
            {
                _logger.LogWarning($"Category with name {name} not found");
                return NotFound(new ResponseDto<CategoryDto>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    StatusMessage = "Category not found",
                    Data = null
                });
            }

            var response = new ResponseDto<CategoryDto>
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Category retrieved successfully",
                Data = category
            };
            return Ok(response);
        }

        [HttpPost]
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto<CategoryDto>>> Create([FromBody] CreateCategoryDto categoryDto)
        {
            _logger.LogInformation($"Creating new category with name: {categoryDto.Name}");
            var createdCategory = await _categoryService.CreateAsync(categoryDto);

            if (createdCategory == null)
            {
                _logger.LogError("Error creating category");
                return new ResponseDto<CategoryDto>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error creating category",
                    Errors = new List<string> { "An unexpected error occurred" },
                    Data = null
                };

            }
            var response = new ResponseDto<CategoryDto>
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
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto<CategoryDto>>> Update(int id, [FromBody] UpdateCategoryDto categoryDto)
        {
            _logger.LogInformation($"Updating category with Id: {id}");
            var updatedCategory = await _categoryService.UpdateAsync(id, categoryDto);

            if (updatedCategory == null)
            {
                _logger.LogError($"Error updating category with ID {id}");
                return new ResponseDto<CategoryDto>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error updating category",
                    Errors = new List<string> { "An unexpected error occurred" },
                    Data = null
                };

            }

            var response = new ResponseDto<CategoryDto>
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
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto<bool>>> Delete(int id)
        {

            _logger.LogInformation($"Deleting category with ID: {id}");
            var deleted = await _categoryService.DeleteAsync(id);

            if (!deleted)
            {
                _logger.LogWarning($"Category with ID {id} not found for deletion");
                var notFoundResponse = new ResponseDto<bool>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    StatusMessage = "Category not found",
                    Errors = new List<string> { $"Category with ID {id} not found" },
                    Data = false
                };
                return NotFound(notFoundResponse);
            }

            var response = new ResponseDto<bool>
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Category deleted successfully",
                Data = true
            };
            return Ok(response);

        }

        [HttpGet]
        [Route("exists-by-id/{id}")]
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto<bool>>> ExistsById(int id)
        {

            var exists = await _categoryService.ExistsAsync(id);

            if (exists)
            {
                var response = new ResponseDto<bool>
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Category exists",
                    Data = true
                };
                return Ok(response);
            }

            var notFoundResponse = new ResponseDto<bool>
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status404NotFound,
                StatusMessage = "Category not found",
                Data = false
            };
            return NotFound(notFoundResponse);


        }

        [HttpGet]
        [Route("exists-by-name/{name}")]
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto<bool>>> ExistsByName(string name)
        {
            var exists = await _categoryService.ExistsByNameAsync(name);

            if (exists)
            {
                var response = new ResponseDto<bool>
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Category exists",
                    Data = true
                };
                return Ok(response);
            }

            var notFoundResponse = new ResponseDto<bool>
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