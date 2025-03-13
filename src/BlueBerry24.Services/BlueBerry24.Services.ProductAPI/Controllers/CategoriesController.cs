using AutoMapper;
using BlueBerry24.Services.CouponAPI.Models.DTOs;
using BlueBerry24.Services.ProductAPI.Exceptions;
using BlueBerry24.Services.ProductAPI.Models.DTOs.CategoryDtos;
using BlueBerry24.Services.ProductAPI.Models.DTOs.ProductDtos;
using BlueBerry24.Services.ProductAPI.Services;
using BlueBerry24.Services.ProductAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlueBerry24.Services.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoriesController> _logger;
        public CategoriesController(ILogger<CategoriesController> logger,
            ICategoryService categoryService, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _categoryService = categoryService;
        }


        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetAll()
        {
            _logger.LogInformation("Getting all categories");
            try
            {
                var categories = await _categoryService.GetAllAsync();
                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Categories retrieved successfully",
                    Data = categories
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all categories");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error retrieving categories",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("{id:guid}", Name = "GetCategoryById")]
        public async Task<ActionResult<ResponseDto>> GetById(string id)
        {
            try
            {
                _logger.LogInformation($"Getting category with ID: {id}");
                var category = await _categoryService.GetByIdAsync(id);
                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Category retrieved successfully",
                    Data = category
                };
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, $"Category with ID {id} not found");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    StatusMessage = "Category not found",
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
                    StatusMessage = "Error retrieving category",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        [HttpGet("name/{name}", Name = "GetCategoryByName")]
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

            try
            {
                _logger.LogInformation($"Getting category with name: {name}");
                var product = await _categoryService.GetByNameAsync(name);
                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Category retrieved successfully",
                    Data = product
                };
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, $"Category with name {name} not found");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    StatusMessage = "Category not found",
                    Errors = new List<string> { ex.Message }
                };
                return NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving category with name {name}");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error retrieving category",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto>> Create([FromBody] CreateCategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                var badRequestResponse = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = "Invalid request",
                    Errors = new List<string> { "Category data is required" }
                };
                return BadRequest(badRequestResponse);
            }

            try
            {
                _logger.LogInformation($"Creating new category with name: {categoryDto.Name}");
                var createdCategory = await _categoryService.CreateAsync(categoryDto);
                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status201Created,
                    StatusMessage = "Category created successfully",
                    Data = createdCategory
                };
                return CreatedAtRoute("GetCategoryById", new { id = createdCategory.Id }, response);
            }
            catch (ValidateException ex)
            {
                _logger.LogWarning(ex, "Validation failed when creating category");
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
                _logger.LogWarning(ex, $"Category with name {categoryDto.Name} already exists");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    StatusMessage = "Category already exists",
                    Errors = new List<string> { ex.Message }
                };
                return Conflict(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error creating category",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ResponseDto>> Update(string id, [FromBody] UpdateCategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                var badRequestResponse = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    StatusMessage = "Invalid request",
                    Errors = new List<string> { "Category data is required" }
                };
                return BadRequest(badRequestResponse);
            }

            try
            {
                _logger.LogInformation($"Updating category with Id: {id}");
                var updatedCategory = await _categoryService.UpdateAsync(id, categoryDto);
                var response = new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Category updated successfully",
                    Data = updatedCategory
                };
                return Ok(response);
            }
            catch (ValidateException ex)
            {
                _logger.LogWarning(ex, "Validation failed when updating category");
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
                _logger.LogWarning(ex, $"Category with ID {id} not found");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    StatusMessage = "Category not found",
                    Errors = new List<string> { ex.Message }
                };
                return NotFound(response);
            }
            catch (DuplicateEntityException ex)
            {
                _logger.LogWarning(ex, $"Category with name {categoryDto.Name} already exists");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    StatusMessage = "Category already exists",
                    Errors = new List<string> { ex.Message }
                };
                return Conflict(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating category with ID {id}");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error updating category",
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting category with ID {id}");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error deleting category",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpHead("{id:guid}")]
        [HttpGet("exists/{id:guid}")]
        public async Task<ActionResult<ResponseDto>> ExistsById(string id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking existence of category with ID {id}");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error checking category existence",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }



        [HttpHead("name/{name}")]
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
                    Errors = new List<string> { "Category name cannot be empty" }
                };
                return BadRequest(badRequestResponse);
            }

            try
            {
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking existence of category with name {name}");
                var response = new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error checking category existence",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


    }
}
