using AutoMapper;
using BlueBerry24.Services.CouponAPI.Models.DTOs;
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

        
    }
}
