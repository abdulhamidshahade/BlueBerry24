using BlueBerry24.Services.AuthAPI.Models.DTOs;
using BlueBerry24.Services.AuthAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlueBerry24.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto requestDto)
        {
            if (requestDto == null || !ModelState.IsValid)
            {
                return StatusCode(400, new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Invalid request data.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            try
            {
                var registerResult = await _authService.Register(requestDto);

                if (registerResult.IsSuccess)
                {
                    return StatusCode(201, new ResponseDto
                    {
                        IsSuccess = true,
                        StatusCode = 201,
                        StatusMessage = "User registered successfully.",
                        Data = registerResult.User
                    });
                }

                return StatusCode(400, new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Registration failed.",
                    Errors = new List<string> { registerResult.ErrorMessage }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during user registration.");

                return StatusCode(500, new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "An unexpected error occurred."
                });
            }
        }
    }
}
