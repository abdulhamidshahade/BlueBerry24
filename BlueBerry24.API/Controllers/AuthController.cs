using BlueBerry24.Application.Authorization.Attributes;
using BlueBerry24.Application.Dtos;
using BlueBerry24.Application.Dtos.AuthDtos;
using BlueBerry24.Application.Services.Interfaces.AuthServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlueBerry24.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IUserService _userService;
        public AuthController(IAuthService authService,
                              ILogger<AuthController> logger,
                              IUserService userService) : base(logger)
        {
            _authService = authService;
            _logger = logger;
            _userService = userService;
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
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
        [AllowAnonymous]

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
        [AdminAndAbove]
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
        [AdminAndAbove]
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

        [HttpPost]
        [AdminAndAbove]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto requestDto)
        {
            if (requestDto == null || string.IsNullOrWhiteSpace(requestDto.Token))
            {
                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Invalid token provided."
                });
            }

            try
            {
                //TODO: Token refresh functionality to be implemented
                return Ok(new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "Token refresh functionality to be implemented."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token");
                return StatusCode(500, new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "An error occurred while refreshing the token."
                });
            }
        }

        [HttpPost]
        [Route("logout")]
        [AllRoles]
        public async Task<IActionResult> Logout()
        {
            //TODO: implement token blacklisting or session management
            return Ok(new ResponseDto
            {
                IsSuccess = true,
                StatusCode = 200,
                StatusMessage = "Logged out successfully"
            });
        }

        [HttpGet]
        [AdminAndAbove]
        [Route("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return Ok(new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "Users retrieved successfully.",
                    Data = users
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users");
                return StatusCode(500, new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "An error occurred while retrieving users."
                });
            }
        }

        [HttpGet]
        [AdminAndAbove]
        [Route("users/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                if (user == null)
                {
                    return NotFound(new ResponseDto
                    {
                        IsSuccess = false,
                        StatusCode = 404,
                        StatusMessage = "User not found."
                    });
                }

                return Ok(new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "User retrieved successfully.",
                    Data = user
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving user with ID {id}");
                return StatusCode(500, new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "An error occurred while retrieving the user."
                });
            }
        }

        [HttpGet]
        [Route("me")]
        [AllRoles]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    StatusMessage = "Invalid token"
                });
            }

            var userExists = await _userService.IsUserExistsByIdAsync(int.Parse(userId));

            if (!userExists)
            {
                return NotFound(new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    StatusMessage = "User not found"
                });
            }

            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var userName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
            var userRoles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToList();

            return Ok(new ResponseDto
            {
                IsSuccess = true,
                StatusCode = 200,
                StatusMessage = "User retrieved successfully",
                Data = new
                {
                    Id = int.Parse(userId),
                    Email = userEmail,
                    UserName = userName,
                    Roles = userRoles
                }
            });
        }

        [HttpPost]
        [AdminAndAbove]
        [Route("users/{userId}/lock")]
        public async Task<IActionResult> LockUserAccount(int userId, [FromBody] LockUserRequestDto requestDto = null)
        {
            try
            {
                DateTime? lockoutEnd = requestDto?.LockoutEnd;
                var result = await _userService.LockUserAccountAsync(userId, lockoutEnd);

                if (result)
                {
                    return Ok(new ResponseDto
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        StatusMessage = "User account locked successfully."
                    });
                }

                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Failed to lock user account."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error locking user account for user ID {userId}");
                return StatusCode(500, new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "An error occurred while locking the user account."
                });
            }
        }

        [HttpPost]
        [AdminAndAbove]
        [Route("users/{userId}/unlock")]
        public async Task<IActionResult> UnlockUserAccount(int userId)
        {
            try
            {
                var result = await _userService.UnlockUserAccountAsync(userId);

                if (result)
                {
                    return Ok(new ResponseDto
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        StatusMessage = "User account unlocked successfully."
                    });
                }

                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Failed to unlock user account."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error unlocking user account for user ID {userId}");
                return StatusCode(500, new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "An error occurred while unlocking the user account."
                });
            }
        }

        [HttpPost]
        [AdminAndAbove]
        [Route("users/{userId}/reset-password")]
        public async Task<IActionResult> ResetUserPassword(int userId, [FromBody] ResetPasswordRequestDto requestDto)
        {
            if (requestDto == null || string.IsNullOrWhiteSpace(requestDto.NewPassword))
            {
                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "New password is required."
                });
            }

            try
            {
                var result = await _userService.ResetUserPasswordAsync(userId, requestDto.NewPassword);

                if (result)
                {
                    return Ok(new ResponseDto
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        StatusMessage = "User password reset successfully."
                    });
                }

                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Failed to reset user password."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error resetting password for user ID {userId}");
                return StatusCode(500, new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "An error occurred while resetting the user password."
                });
            }
        }

        [HttpPost]
        [AdminAndAbove]
        [Route("users/{userId}/verify-email")]
        public async Task<IActionResult> VerifyUserEmail(int userId)
        {
            try
            {
                var result = await _userService.VerifyUserEmailAsync(userId);

                if (result)
                {
                    return Ok(new ResponseDto
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        StatusMessage = "User email verified successfully."
                    });
                }

                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Failed to verify user email."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error verifying email for user ID {userId}");
                return StatusCode(500, new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "An error occurred while verifying the user email."
                });
            }
        }

        [HttpPut]
        [AdminAndAbove]
        [Route("users/{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserDto updateUserDto)
        {
            if (updateUserDto == null || !ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Invalid request data.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            try
            {
                var result = await _userService.UpdateUserAsync(userId, updateUserDto);

                if (result)
                {
                    return Ok(new ResponseDto
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        StatusMessage = "User updated successfully."
                    });
                }

                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Failed to update user."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating user with ID {userId}");
                return StatusCode(500, new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "An error occurred while updating the user."
                });
            }
        }

        [HttpPost]
        [AdminAndAbove]
        [Route("users")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            if (createUserDto == null || !ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Invalid request data.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            try
            {
                var user = await _userService.CreateUserAsync(createUserDto);

                if (user != null)
                {
                    return StatusCode(201, new ResponseDto
                    {
                        IsSuccess = true,
                        StatusCode = 201,
                        StatusMessage = "User created successfully.",
                        Data = user
                    });
                }

                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Failed to create user."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "An error occurred while creating the user."
                });
            }
        }

        [HttpDelete]
        [AdminAndAbove]
        [Route("users/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(userId);

                if (result)
                {
                    return Ok(new ResponseDto
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        StatusMessage = "User deleted successfully."
                    });
                }

                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Failed to delete user."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting user with ID {userId}");
                return StatusCode(500, new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "An error occurred while deleting the user."
                });
            }
        }
    }

    public class RefreshTokenRequestDto
    {
        public string Token { get; set; } = string.Empty;
    }

    public class LockUserRequestDto
    {
        public DateTime? LockoutEnd { get; set; }
    }

    public class ResetPasswordRequestDto
    {
        public string NewPassword { get; set; } = string.Empty;
    }
}
