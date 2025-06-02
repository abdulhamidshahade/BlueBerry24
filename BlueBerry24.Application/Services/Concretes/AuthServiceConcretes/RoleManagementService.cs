using AutoMapper;
using BlueBerry24.Application.Dtos.AuthDtos;
using BlueBerry24.Application.Services.Interfaces.AuthServiceInterfaces;
using BlueBerry24.Domain.Constants;
using BlueBerry24.Domain.Entities.AuthEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Services.Concretes.AuthServiceConcretes
{
    public class RoleManagementService : IRoleManagementService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleManagementService> _logger;

        public RoleManagementService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IMapper mapper,
            ILogger<RoleManagementService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(roleName))
                    return false;

                if (await _roleManager.RoleExistsAsync(roleName))
                    return true;

                var role = new ApplicationRole(roleName);
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"Role '{roleName}' created successfully.");
                    return true;
                }

                _logger.LogError($"Failed to create role '{roleName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating role '{roleName}'");
                return false;
            }
        }

        public async Task<bool> DeleteRoleAsync(string roleName)
        {
            try
            {
                var systemRoles = new[] { RoleConstants.SuperAdmin, RoleConstants.Admin };
                if (systemRoles.Contains(roleName))
                {
                    _logger.LogWarning($"Attempt to delete system role '{roleName}' denied.");
                    return false;
                }

                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null)
                    return false;

                var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
                if (usersInRole.Any())
                {
                    _logger.LogWarning($"Cannot delete role '{roleName}' because it is assigned to {usersInRole.Count} user(s).");
                    return false;
                }

                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"Role '{roleName}' deleted successfully.");
                    return true;
                }

                _logger.LogError($"Failed to delete role '{roleName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting role '{roleName}'");
                return false;
            }
        }

        public async Task<bool> AssignRoleToUserAsync(int userId, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    return false;

                if (!await _roleManager.RoleExistsAsync(roleName))
                    await CreateRoleAsync(roleName);

                if (await _userManager.IsInRoleAsync(user, roleName))
                    return true;

                var result = await _userManager.AddToRoleAsync(user, roleName);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"Role '{roleName}' assigned to user '{user.UserName}' successfully.");
                    return true;
                }

                _logger.LogError($"Failed to assign role '{roleName}' to user '{user.UserName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error assigning role '{roleName}' to user with ID '{userId}'");
                return false;
            }
        }

        public async Task<bool> RemoveRoleFromUserAsync(int userId, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    return false;

                if (!await _userManager.IsInRoleAsync(user, roleName))
                    return true;

                if (roleName == RoleConstants.SuperAdmin)
                {
                    var superAdmins = await _userManager.GetUsersInRoleAsync(RoleConstants.SuperAdmin);
                    if (superAdmins.Count <= 1)
                    {
                        _logger.LogWarning($"Cannot remove SuperAdmin role from user '{user.UserName}' as they are the last SuperAdmin.");
                        return false;
                    }
                }

                var result = await _userManager.RemoveFromRoleAsync(user, roleName);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"Role '{roleName}' removed from user '{user.UserName}' successfully.");
                    return true;
                }

                _logger.LogError($"Failed to remove role '{roleName}' from user '{user.UserName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error removing role '{roleName}' from user with ID '{userId}'");
                return false;
            }
        }

        public async Task<List<string>> GetUserRolesAsync(int userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    return new List<string>();

                var roles = await _userManager.GetRolesAsync(user);
                return roles.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting roles for user with ID '{userId}'");
                return new List<string>();
            }
        }

        public async Task<List<ApplicationRoleDto>> GetAllRolesAsync()
        {
            try
            {
                var roles = await _roleManager.Roles.ToListAsync();
                return _mapper.Map<List<ApplicationRoleDto>>(roles) ;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all roles");
                return null;
            }
        }

        public async Task<List<ApplicationUserDto>> GetUsersInRoleAsync(string roleName)
        {
            try
            {
                var users = await _userManager.GetUsersInRoleAsync(roleName);
                return _mapper.Map<List<ApplicationUserDto>>(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting users in role '{roleName}'");
                return new List<ApplicationUserDto>();
            }
        }

        public async Task<bool> IsUserInRoleAsync(int userId, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    return false;

                return await _userManager.IsInRoleAsync(user, roleName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking if user with ID '{userId}' is in role '{roleName}'");
                return false;
            }
        }

        public async Task InitializeDefaultRolesAsync()
        {
            try
            {
                foreach (var role in RoleConstants.AllRoles)
                {
                    await CreateRoleAsync(role);
                }

                _logger.LogInformation("Default roles initialized successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing default roles");
            }
        }

        public async Task<List<ApplicationUserWithRolesDto>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                var userDtos = new List<ApplicationUserWithRolesDto>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var userDto = _mapper.Map<ApplicationUserWithRolesDto>(user);
                    userDto.Roles = roles.ToList();
                    userDtos.Add(userDto);
                }

                return userDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                return new List<ApplicationUserWithRolesDto>();
            }
        }

        public async Task<ApplicationUserWithRolesDto> GetUserByIdAsync(int userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    return null;

                var roles = await _userManager.GetRolesAsync(user);
                var userDto = _mapper.Map<ApplicationUserWithRolesDto>(user);
                userDto.Roles = roles.ToList();

                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting user with ID '{userId}'");
                return null;
            }
        }

        public async Task<RoleStatsDto> GetRoleStatsAsync()
        {
            try
            {
                var totalRoles = await _roleManager.Roles.CountAsync();
                var totalUsers = await _userManager.Users.CountAsync();

                var usersWithRoles = 0;
                var users = await _userManager.Users.ToListAsync();

                foreach (var user in users)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    if (userRoles.Any())
                    {
                        usersWithRoles++;
                    }
                }

                var usersWithoutRoles = totalUsers - usersWithRoles;

                return new RoleStatsDto
                {
                    TotalRoles = totalRoles,
                    TotalUsers = totalUsers,
                    UsersWithRoles = usersWithRoles,
                    UsersWithoutRoles = usersWithoutRoles
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting role statistics");
                return new RoleStatsDto();
            }
        }

        public async Task<bool> UpdateRoleAsync(string oldRoleName, string newRoleName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(oldRoleName) || string.IsNullOrWhiteSpace(newRoleName))
                    return false;

                var oldRole = await _roleManager.FindByNameAsync(oldRoleName);
                if (oldRole == null)
                    return false;

                if (await _roleManager.RoleExistsAsync(newRoleName))
                    return false;

                var systemRoles = new[] { RoleConstants.SuperAdmin, RoleConstants.Admin };
                if (systemRoles.Contains(oldRoleName))
                {
                    _logger.LogWarning($"Attempt to update system role '{oldRoleName}' denied.");
                    return false;
                }

                oldRole.Name = newRoleName;
                oldRole.NormalizedName = newRoleName.ToUpperInvariant();

                var result = await _roleManager.UpdateAsync(oldRole);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"Role '{oldRoleName}' updated to '{newRoleName}' successfully.");
                    return true;
                }

                _logger.LogError($"Failed to update role '{oldRoleName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating role '{oldRoleName}' to '{newRoleName}'");
                return false;
            }
        }

        public async Task<BulkAssignmentResultDto> BulkAssignRoleAsync(List<int> userIds, string roleName)
        {
            var result = new BulkAssignmentResultDto
            {
                TotalUsers = userIds?.Count ?? 0
            };

            try
            {
                if (userIds == null || !userIds.Any() || string.IsNullOrWhiteSpace(roleName))
                {
                    result.ErrorMessages.Add("Invalid input parameters");
                    return result;
                }

                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await CreateRoleAsync(roleName);
                }

                foreach (var userId in userIds)
                {
                    try
                    {
                        var success = await AssignRoleToUserAsync(userId, roleName);
                        if (success)
                        {
                            result.SuccessfulAssignments++;
                        }
                        else
                        {
                            result.FailedAssignments++;
                            result.FailedUserIds.Add(userId.ToString());
                            result.ErrorMessages.Add($"Failed to assign role to user {userId}");
                        }
                    }
                    catch (Exception ex)
                    {
                        result.FailedAssignments++;
                        result.FailedUserIds.Add(userId.ToString());
                        result.ErrorMessages.Add($"Error assigning role to user {userId}: {ex.Message}");
                        _logger.LogError(ex, $"Error in bulk assignment for user {userId}");
                    }
                }

                _logger.LogInformation($"Bulk assignment completed: {result.SuccessfulAssignments} successful, {result.FailedAssignments} failed");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in bulk role assignment");
                result.ErrorMessages.Add($"Bulk assignment failed: {ex.Message}");
                return result;
            }
        }
    }
}
