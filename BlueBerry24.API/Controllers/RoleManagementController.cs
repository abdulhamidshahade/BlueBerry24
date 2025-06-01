﻿using BlueBerry24.Application.Authorization.Attributes;
using BlueBerry24.Application.Dtos;
using BlueBerry24.Application.Dtos.AuthDtos;
using BlueBerry24.Application.Services.Interfaces.AuthServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlueBerry24.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SuperAdminOnly]
    public class RoleManagementController : BaseController
    {
        private readonly IRoleManagementService _roleManagementService;
        private readonly ILogger<RoleManagementController> _logger;
        

        public RoleManagementController(
            IRoleManagementService roleManagementService,
            ILogger<RoleManagementController> logger) : base(logger)
        {
            _roleManagementService = roleManagementService;
            _logger = logger;
        }


        [HttpPost("roles")]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            var result = await _roleManagementService.CreateRoleAsync(roleName);

            if (result)
            {
                return Ok(new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = $"Role '{roleName}' created successfully."
                });
            }

            return BadRequest(new ResponseDto
            {
                IsSuccess = false,
                StatusCode = 400,
                StatusMessage = $"Failed to create role '{roleName}'."
            });
        }

        [HttpDelete("roles/{roleName}")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var result = await _roleManagementService.DeleteRoleAsync(roleName);

            if (result)
            {
                return Ok(new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = $"Role '{roleName}' deleted successfully."
                });
            }

            return BadRequest(new ResponseDto
            {
                IsSuccess = false,
                StatusCode = 400,
                StatusMessage = $"Failed to delete role '{roleName}'."
            });
        }

        [HttpPost("users/{userId}/roles/{roleName}")]
        public async Task<IActionResult> AssignRoleToUser(int userId, string roleName)
        {
            var result = await _roleManagementService.AssignRoleToUserAsync(userId, roleName);

            if (result)
            {
                return Ok(new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = $"Role '{roleName}' assigned to user successfully."
                });
            }

            return BadRequest(new ResponseDto
            {
                IsSuccess = false,
                StatusCode = 400,
                StatusMessage = $"Failed to assign role '{roleName}' to user."
            });
        }

        [HttpDelete("users/{userId}/roles/{roleName}")]
        public async Task<IActionResult> RemoveRoleFromUser(int userId, string roleName)
        {
            var result = await _roleManagementService.RemoveRoleFromUserAsync(userId, roleName);

            if (result)
            {
                return Ok(new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = $"Role '{roleName}' removed from user successfully."
                });
            }

            return BadRequest(new ResponseDto
            {
                IsSuccess = false,
                StatusCode = 400,
                StatusMessage = $"Failed to remove role '{roleName}' from user."
            });
        }

        [HttpGet("users/{userId}/roles")]
        public async Task<IActionResult> GetUserRoles(int userId)
        {
            var roles = await _roleManagementService.GetUserRolesAsync(userId);

            return Ok(new ResponseDto
            {
                IsSuccess = true,
                StatusCode = 200,
                StatusMessage = "User roles retrieved successfully.",
                Data = roles
            });
        }

        [HttpGet("roles")]
        [SuperAdminOnly]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManagementService.GetAllRolesAsync();

            return Ok(new ResponseDto
            {
                IsSuccess = true,
                StatusCode = 200,
                StatusMessage = "All roles retrieved successfully.",
                Data = roles
            });
        }

        [HttpGet("roles/{roleName}/users")]
        public async Task<IActionResult> GetUsersInRole(string roleName)
        {
            var users = await _roleManagementService.GetUsersInRoleAsync(roleName);

            return Ok(new ResponseDto
            {
                IsSuccess = true,
                StatusCode = 200,
                StatusMessage = $"Users in role '{roleName}' retrieved successfully.",
                Data = users
            });
        }

        [HttpGet("users/{userId}/roles/{roleName}/check")]
        public async Task<IActionResult> IsUserInRole(int userId, string roleName)
        {
            var isInRole = await _roleManagementService.IsUserInRoleAsync(userId, roleName);

            return Ok(new ResponseDto
            {
                IsSuccess = true,
                StatusCode = 200,
                StatusMessage = "Role check completed.",
                Data = new { UserId = userId, RoleName = roleName, IsInRole = isInRole }
            });
        }

        [HttpPost("initialize-default-roles")]
        public async Task<IActionResult> InitializeDefaultRoles()
        {
            await _roleManagementService.InitializeDefaultRolesAsync();

            return Ok(new ResponseDto
            {
                IsSuccess = true,
                StatusCode = 200,
                StatusMessage = "Default roles initialized successfully."
            });
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _roleManagementService.GetAllUsersAsync();

            return Ok(new ResponseDto
            {
                IsSuccess = true,
                StatusCode = 200,
                StatusMessage = "All users retrieved successfully.",
                Data = users
            });
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var user = await _roleManagementService.GetUserByIdAsync(userId);

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

        [HttpGet("stats")]
        public async Task<IActionResult> GetRoleStats()
        {
            var stats = await _roleManagementService.GetRoleStatsAsync();

            return Ok(new ResponseDto
            {
                IsSuccess = true,
                StatusCode = 200,
                StatusMessage = "Role statistics retrieved successfully.",
                Data = stats
            });
        }

        [HttpPut("roles/{oldRoleName}")]
        public async Task<IActionResult> UpdateRole(string oldRoleName, [FromBody] string newRoleName)
        {
            var result = await _roleManagementService.UpdateRoleAsync(oldRoleName, newRoleName);

            if (result)
            {
                return Ok(new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = $"Role '{oldRoleName}' updated to '{newRoleName}' successfully."
                });
            }

            return BadRequest(new ResponseDto
            {
                IsSuccess = false,
                StatusCode = 400,
                StatusMessage = $"Failed to update role '{oldRoleName}'."
            });
        }

        [HttpPost("bulk-assign")]
        public async Task<IActionResult> BulkAssignRole([FromBody] BulkAssignRoleRequest request)
        {
            if (request == null || request.UserIds == null || !request.UserIds.Any() || string.IsNullOrWhiteSpace(request.RoleName))
            {
                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "Invalid request. User IDs and role name are required."
                });
            }

            var result = await _roleManagementService.BulkAssignRoleAsync(request.UserIds, request.RoleName);

            if (result.IsSuccess)
            {
                return Ok(new ResponseDto
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = $"Successfully assigned role '{request.RoleName}' to {result.SuccessfulAssignments} users.",
                    Data = result
                });
            }

            return BadRequest(new ResponseDto
            {
                IsSuccess = false,
                StatusCode = 400,
                StatusMessage = $"Bulk assignment partially failed. {result.SuccessfulAssignments} succeeded, {result.FailedAssignments} failed.",
                Data = result
            });
        }
    }
}
