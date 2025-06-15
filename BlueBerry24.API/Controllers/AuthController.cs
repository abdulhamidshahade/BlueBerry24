﻿using BlueBerry24.Application.Authorization.Attributes;
using BlueBerry24.Application.Dtos;
using BlueBerry24.Application.Dtos.AuthDtos;
using BlueBerry24.Application.Services.Interfaces.AuthServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces;
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
        private readonly ICartService _cartService;
        public AuthController(IAuthService authService,
                              ILogger<AuthController> logger,
                              IUserService userService,
                              ICartService cartService) : base(logger)
        {
            _authService = authService;
            _logger = logger;
            _userService = userService;
            _cartService = cartService;
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto requestDto)
        {
            if (requestDto == null || !ModelState.IsValid)
            {
                return StatusCode(400, new ResponseDto<RegisterResponseDto>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Invalid request data.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList(),
                    Data = null
                });
            }
            var registerResult = await _authService.Register(requestDto);

            if (registerResult.IsSuccess)
            {
                return StatusCode(201, new ResponseDto<object>
                {
                    IsSuccess = true,
                    StatusCode = 201,
                    StatusMessage = "User registered successfully.",
                    Data = registerResult.User
                });
            }

            return StatusCode(400, new ResponseDto<object>
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
                return StatusCode(400, new ResponseDto<LoginResponseDto>
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
                var user = await _userService.GetUserByEmail(requestDto.Email);
                await _cartService.MergeCartAsync(user.Id, GetSessionId());

                return StatusCode(200, new ResponseDto<LoginResponseDto>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "User logged in successfully.",
                    Data = loginResult
                });
            }

            if (!string.IsNullOrEmpty(loginResult.ErrorMessage))
            {
                return StatusCode(403, new ResponseDto<LoginResponseDto>
                {
                    IsSuccess = false,
                    StatusCode = 403,
                    StatusMessage = loginResult.ErrorMessage,
                });
            }

            return Unauthorized(new ResponseDto<LoginResponseDto>
            {
                IsSuccess = false,
                StatusCode = 401,
                StatusMessage = "Invalid email or password",
            });
        }

        [HttpPost]
        [Route("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto requestDto)
        {
            if (requestDto == null || !ModelState.IsValid)
            {
                return StatusCode(400, new ResponseDto<object>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Invalid request data.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            try
            {
                var result = await _authService.ForgotPasswordAsync(requestDto);

                if (result)
                {
                    return Ok(new ResponseDto<object>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        StatusMessage = "If the email address exists in our system, you will receive a password reset link."
                    });
                }

                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "An error occurred while processing your request."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing forgot password request");
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "An error occurred while processing your request."
                });
            }
        }

        [HttpPost]
        [Route("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromBody] EmailConfirmationDto requestDto)
        {
            if (requestDto == null || !ModelState.IsValid)
            {
                return StatusCode(400, new ResponseDto<object>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Invalid request data.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            try
            {
                var result = await _authService.ConfirmEmailAsync(requestDto);

                if (result)
                {
                    return Ok(new ResponseDto<object>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        StatusMessage = "Email confirmed successfully. You can now sign in to your account."
                    });
                }

                return BadRequest(new ResponseDto<object>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Failed to confirm email. The confirmation link may be invalid or expired."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming email");
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "An error occurred while confirming your email."
                });
            }
        }

        [HttpPost]
        [Route("resend-confirmation")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendConfirmation([FromBody] ForgotPasswordRequestDto requestDto)
        {
            if (requestDto == null || !ModelState.IsValid)
            {
                return StatusCode(400, new ResponseDto<object>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Invalid request data.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            try
            {
                var result = await _authService.ResendConfirmationEmailAsync(requestDto.Email);

                if (result)
                {
                    return Ok(new ResponseDto<object>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        StatusMessage = "If the email address exists in our system, a new confirmation link has been sent."
                    });
                }

                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "An error occurred while sending the confirmation email."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resending confirmation email");
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "An error occurred while sending the confirmation email."
                });
            }
        }

        [HttpPost]
        [Route("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto requestDto)
        {
            if (requestDto == null || !ModelState.IsValid)
            {
                return StatusCode(400, new ResponseDto<object>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Invalid request data.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            try
            {
                var result = await _authService.ResetPasswordAsync(requestDto);

                if (result)
                {
                    return Ok(new ResponseDto<object>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        StatusMessage = "Password has been reset successfully."
                    });
                }

                return BadRequest(new ResponseDto<object>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Failed to reset password. Please check your email and reset token."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing password reset");
                return StatusCode(500, new ResponseDto<object>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "An error occurred while processing your request."
                });
            }
        }

        [HttpGet]
        [AdminAndAbove]
        [Route("exists/{id}")]
        public async Task<ActionResult<ResponseDto<object>>> IsUserExistsById(int id)
        {
            var exists = await _userService.IsUserExistsByIdAsync(id);

            if (!exists)
            {
                return NotFound(new ResponseDto<object>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    StatusMessage = "The user is not exists by Id"
                });
            }

            return Ok(new ResponseDto<object>
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
                return NotFound(new ResponseDto<object>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    StatusMessage = "The user is not exists by Email"
                });
            }

            return Ok(new ResponseDto<object>
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
                return BadRequest(new ResponseDto<object>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Invalid token provided."
                });
            }

            try
            {
                //TODO: Token refresh functionality to be implemented
                return Ok(new ResponseDto<object>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = "Token refresh functionality to be implemented."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token");
                return StatusCode(500, new ResponseDto<object>
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
            return Ok(new ResponseDto<object>
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
                return Ok(new ResponseDto<List<ApplicationUserDto>>
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
                return StatusCode(500, new ResponseDto<List<ApplicationUserDto>>
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
                    return NotFound(new ResponseDto<ApplicationUserDto>
                    {
                        IsSuccess = false,
                        StatusCode = 404,
                        StatusMessage = "User not found."
                    });
                }

                return Ok(new ResponseDto<ApplicationUserDto>
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
                return StatusCode(500, new ResponseDto<ApplicationUserDto>
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
                return Unauthorized(new ResponseDto<object>
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    StatusMessage = "Invalid token"
                });
            }

            var userExists = await _userService.IsUserExistsByIdAsync(int.Parse(userId));

            if (!userExists)
            {
                return NotFound(new ResponseDto<object>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    StatusMessage = "User not found"
                });
            }

            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var userName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
            var userRoles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToList();

            return Ok(new ResponseDto<object>
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
                    return Ok(new ResponseDto<bool>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        StatusMessage = "User account locked successfully."
                    });
                }

                return BadRequest(new ResponseDto<bool>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Failed to lock user account."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error locking user account for user ID {userId}");
                return StatusCode(500, new ResponseDto<bool>
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
                    return Ok(new ResponseDto<bool>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        StatusMessage = "User account unlocked successfully."
                    });
                }

                return BadRequest(new ResponseDto<bool>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Failed to unlock user account."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error unlocking user account for user ID {userId}");
                return StatusCode(500, new ResponseDto<bool>
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
                return BadRequest(new ResponseDto<bool>
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
                    return Ok(new ResponseDto<bool>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        StatusMessage = "User password reset successfully."
                    });
                }

                return BadRequest(new ResponseDto<bool>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Failed to reset user password."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error resetting password for user ID {userId}");
                return StatusCode(500, new ResponseDto<bool>
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
                    return Ok(new ResponseDto<bool>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        StatusMessage = "User email verified successfully."
                    });
                }

                return BadRequest(new ResponseDto<bool>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Failed to verify user email."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error verifying email for user ID {userId}");
                return StatusCode(500, new ResponseDto<bool>
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
                return BadRequest(new ResponseDto<bool>
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
                    return Ok(new ResponseDto<bool>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        StatusMessage = "User updated successfully."
                    });
                }

                return BadRequest(new ResponseDto<bool>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Failed to update user."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating user with ID {userId}");
                return StatusCode(500, new ResponseDto<bool>
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
                return BadRequest(new ResponseDto<ApplicationUserDto>
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
                    return StatusCode(201, new ResponseDto<ApplicationUserDto>
                    {
                        IsSuccess = true,
                        StatusCode = 201,
                        StatusMessage = "User created successfully.",
                        Data = user
                    });
                }

                return BadRequest(new ResponseDto<ApplicationUserDto>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Failed to create user."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, new ResponseDto<ApplicationUserDto>
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
                    return Ok(new ResponseDto<bool>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        StatusMessage = "User deleted successfully."
                    });
                }

                return BadRequest(new ResponseDto<bool>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Failed to delete user."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting user with ID {userId}");
                return StatusCode(500, new ResponseDto<bool>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = "An error occurred while deleting the user."
                });
            }
        }
    }
}
