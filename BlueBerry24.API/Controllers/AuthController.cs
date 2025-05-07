using BlueBerry24.Application.Dtos;
using BlueBerry24.Application.Dtos.AuthDtos;
using BlueBerry24.Application.Services.Interfaces.AuthServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlueBerry24.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IUserService _userService;
        public AuthController(IAuthService authService,
                              ILogger<AuthController> logger, 
                              IUserService userService)
        {
            _authService = authService;
            _logger = logger;
            _userService = userService;
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


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequestDto requestDto)
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

            var loginResult = await _authService.Login(requestDto);

            if (loginResult.Token != string.Empty)
            {
                return StatusCode(200, new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "User logged in successfully.",
                    Data = loginResult
                });
            }

            return Unauthorized(new ResponseDto
            {
                IsSuccess = false,
                StatusCode = 401,
                StatusMessage = "Invalid email or password",
            });
        }



        [HttpGet]
        [Route("exists/{id}")]
        public async Task<ActionResult<ResponseDto>> IsUserExistsById(int id)
        {
            var exists = await _userService.IsUserExistsByIdAsync(id);

            if (!exists)
            {
                return NotFound(new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    StatusMessage = "The user is not exists by Id"
                });
            }

            return Ok(new ResponseDto
            {
                IsSuccess = true,
                StatusCode = 200,
                StatusMessage = "The user is exists by Id"
            });
        }


        [HttpGet]
        [Route("exists/email-address/{emailAddress}")]
        public async Task<IActionResult> IsUserExistsByEmail(string emailAddress)
        {
            var exists = await _userService.IsUserExistsByEmailAsync(emailAddress);

            if (!exists)
            {
                return NotFound(new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    StatusMessage = "The user is not exists by Email"
                });
            }

            return Ok(new ResponseDto
            {
                IsSuccess = true,
                StatusCode = 200,
                StatusMessage = "The user is exists by Email"
            });
        }
    }
}
