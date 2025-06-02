using BlueBerry24.Application.Dtos.AuthDtos;
using BlueBerry24.Application.Halpers;
using BlueBerry24.Application.Services.Interfaces.AuthServiceInterfaces;
using BlueBerry24.Domain.Entities.AuthEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlueBerry24.Application.Services.Concretes.AuthServiceConcretes
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AuthService> _logger;
        private readonly ITokenService _tokenService;
        private readonly IRoleManagementService _roleService;

        public AuthService(UserManager<ApplicationUser> userManager,
                           ILogger<AuthService> logger,
                           ITokenService tokenService,
                           IRoleManagementService roleService)
        {
            _userManager = userManager;
            _logger = logger;
            _tokenService = tokenService;
            _roleService = roleService;
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

    }
}
