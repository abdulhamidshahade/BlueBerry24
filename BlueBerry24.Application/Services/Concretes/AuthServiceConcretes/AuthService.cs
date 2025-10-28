using BlueBerry24.Application.Dtos.AuthDtos;
using BlueBerry24.Application.Halpers;
using BlueBerry24.Application.Services.Interfaces.AuthServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.EmailServiceInterfaces;
using BlueBerry24.Domain.Entities.AuthEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BlueBerry24.Application.Services.Concretes.AuthServiceConcretes
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AuthService> _logger;
        private readonly ITokenService _tokenService;
        private readonly IRoleManagementService _roleService;
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager,
                           ILogger<AuthService> logger,
                           ITokenService tokenService,
                           IRoleManagementService roleService,
                           IMailService mailService,
                           IConfiguration configuration)
        {
            _userManager = userManager;
            _logger = logger;
            _tokenService = tokenService;
            _roleService = roleService;
            _mailService = mailService;
            _configuration = configuration;
        }

        public async Task<RegisterResponseDto> Register(RegisterRequestDto requestDto)
        {
            if (requestDto == null)
            {
                return new RegisterResponseDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid registration request."
                };
            }

            try
            {
                ApplicationUser user = new()
                {
                    FirstName = requestDto.FirstName,
                    LastName = requestDto.LastName,
                    UserName = requestDto.UserName,
                    Email = requestDto.Email,
                    NormalizedEmail = EmailNormalizer.NormalizeEmail(requestDto.Email)
                };

                var result = await _userManager.CreateAsync(user, requestDto.Password);

                if (result.Succeeded)
                {
                    string defaultRole = "User";


                    bool IsRoleAssignmentSuccess = await _roleService.AssignRoleToUserAsync(user.Id, defaultRole);

                    if (!IsRoleAssignmentSuccess)
                    {
                        return new RegisterResponseDto
                        {
                            IsSuccess = false,
                            ErrorMessage = "Failed to assign role to the user!"
                        };
                    }

                    var userRoles = await _userManager.GetRolesAsync(user);

                    try
                    {
                        var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var baseUrl = _configuration["App:BaseUrl"] ?? "https://localhost:3000";
                        var confirmationUrl = $"{baseUrl}/auth/confirm-email";

                        await _mailService.SendEmailConfirmationAsync(user.Email, confirmationToken, confirmationUrl, user.UserName);
                        _logger.LogInformation("Email confirmation sent successfully to user {Email}", user.Email);
                    }
                    catch (Exception emailEx)
                    {
                        _logger.LogError(emailEx, "Failed to send email confirmation to user {Email}", user.Email);
                    }

                    ApplicationUserDto userDto = new()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        Roles = userRoles.ToList()
                    };

                    return new RegisterResponseDto
                    {
                        IsSuccess = true,
                        User = userDto
                    };
                }

                return new RegisterResponseDto
                {
                    IsSuccess = false,
                    ErrorMessage = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering the user.");

                return new RegisterResponseDto
                {
                    IsSuccess = false,
                    ErrorMessage = "An unexpected error occurred."
                };
            }
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto requestDto)
        {
            var normalizedEmail = requestDto.Email.Trim().ToLower();
            var user = await _userManager.FindByEmailAsync(normalizedEmail);

            if (user == null)
            {
                _logger.LogWarning("User not found for email: {Email}", normalizedEmail);
                return new LoginResponseDto { User = null, Token = "" };
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, requestDto.Password);

            if (!isPasswordCorrect)
            {
                _logger.LogWarning("Invalid password for email: {Email}", normalizedEmail);
                return new LoginResponseDto { User = null, Token = "" };
            }

            if (!user.EmailConfirmed)
            {
                _logger.LogWarning("Login attempted with unconfirmed email: {Email}", normalizedEmail);
                return new LoginResponseDto { User = null, Token = "", ErrorMessage = "Email not confirmed. Please check your email and confirm your account." };
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            ApplicationUserDto userDto = new()
            {
                Email = requestDto.Email,
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Roles = userRoles.ToList()
            };

            var token = await _tokenService.GenerateToken(user);

            return new LoginResponseDto()
            {
                User = userDto,
                Token = token
            };
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequestDto requestDto)
        {
            try
            {
                var normalizedEmail = requestDto.Email.Trim().ToLower();
                var user = await _userManager.FindByEmailAsync(normalizedEmail);

                if (user == null)
                {
                    _logger.LogWarning("Password reset requested for non-existent email: {Email}", normalizedEmail);
                    return true;
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var baseUrl = _configuration["App:BaseUrl"] ?? "https://localhost:3000";
                var resetUrl = $"{baseUrl}/auth/reset-password";

                try
                {
                    await _mailService.SendPasswordResetEmailAsync(user.Email, token, resetUrl);
                    _logger.LogInformation("Password reset email sent successfully to user {Email}", user.Email);
                }
                catch (Exception emailEx)
                {
                    _logger.LogError(emailEx, "Failed to send password reset email to user {Email}. Token: {Token}",
                        user.Email, token);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing forgot password request for email: {Email}",
                    requestDto.Email);
                return false;
            }
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto requestDto)
        {
            try
            {
                var normalizedEmail = requestDto.Email.Trim().ToLower();
                var user = await _userManager.FindByEmailAsync(normalizedEmail);

                if (user == null)
                {
                    _logger.LogWarning("Password reset attempted for non-existent email: {Email}", normalizedEmail);
                    return false;
                }

                var result = await _userManager.ResetPasswordAsync(user, requestDto.Token, requestDto.NewPassword);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Password reset successful for user {Email}", user.Email);
                    return true;
                }

                _logger.LogWarning("Password reset failed for user {Email}. Errors: {Errors}",
                    user.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while resetting password for email: {Email}",
                    requestDto.Email);
                return false;
            }
        }

        public async Task<bool> ConfirmEmailAsync(EmailConfirmationDto confirmationDto)
        {
            try
            {
                var normalizedEmail = confirmationDto.Email.Trim().ToLower();
                var user = await _userManager.FindByEmailAsync(normalizedEmail);

                if (user == null)
                {
                    _logger.LogWarning("Email confirmation attempted for non-existent email: {Email}", normalizedEmail);
                    return false;
                }

                if (user.EmailConfirmed)
                {
                    _logger.LogInformation("Email already confirmed for user {Email}", user.Email);
                    return true;
                }

                var result = await _userManager.ConfirmEmailAsync(user, confirmationDto.Token);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Email confirmed successfully for user {Email}", user.Email);
                    return true;
                }

                _logger.LogWarning("Email confirmation failed for user {Email}. Errors: {Errors}",
                    user.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while confirming email for: {Email}",
                    confirmationDto.Email);
                return false;
            }
        }

        public async Task<bool> ResendConfirmationEmailAsync(string email)
        {
            try
            {
                var normalizedEmail = email.Trim().ToLower();
                var user = await _userManager.FindByEmailAsync(normalizedEmail);

                if (user == null)
                {
                    _logger.LogWarning("Resend confirmation requested for non-existent email: {Email}", normalizedEmail);
                    return true;
                }

                if (user.EmailConfirmed)
                {
                    _logger.LogInformation("Resend confirmation requested for already confirmed email: {Email}", user.Email);
                    return true;
                }

                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var baseUrl = _configuration["App:BaseUrl"] ?? "https://localhost:3000";
                var confirmationUrl = $"{baseUrl}/auth/confirm-email";

                try
                {
                    await _mailService.SendEmailConfirmationAsync(user.Email, confirmationToken, confirmationUrl, user.UserName);
                    _logger.LogInformation("Confirmation email resent successfully to user {Email}", user.Email);
                }
                catch (Exception emailEx)
                {
                    _logger.LogError(emailEx, "Failed to resend confirmation email to user {Email}", user.Email);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while resending confirmation email for: {Email}", email);
                return false;
            }
        }
    }
}
