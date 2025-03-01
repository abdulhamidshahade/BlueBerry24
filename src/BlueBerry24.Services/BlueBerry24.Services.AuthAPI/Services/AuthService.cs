﻿using BlueBerry24.Services.AuthAPI.Data;
using BlueBerry24.Services.AuthAPI.Models.DTOs;
using BlueBerry24.Services.AuthAPI.Models;
using BlueBerry24.Services.AuthAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using BlueBerry24.Services.AuthAPI.Halpers;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry24.Services.AuthAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRoleService _roleService;
        private readonly ILogger<AuthService> _logger;
        private readonly ITokenService _tokenService;

        public AuthService(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IRoleService roleService,
            ILogger<AuthService> logger,
            ITokenService tokenService)
        {
            _context = context;
            _userManager = userManager;
            _roleService = roleService;
            _logger = logger;
            _tokenService = tokenService;
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

                    if (_roleService.IsValidRole(defaultRole))
                    {
                        bool IsRoleAssignmentSuccess = await _roleService.AssignRoleToUserAsync(user, defaultRole);

                        if (!IsRoleAssignmentSuccess)
                        {
                            return new RegisterResponseDto
                            {
                                IsSuccess = false,
                                ErrorMessage = "Failed to assign role to the user!"
                            };
                        }
                    }

                    ApplicationUserDto userDto = new()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
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
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(e => e.Email.Trim().ToLower() == normalizedEmail);

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

            ApplicationUserDto userDto = new()
            {
                Email = requestDto.Email,
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName
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
