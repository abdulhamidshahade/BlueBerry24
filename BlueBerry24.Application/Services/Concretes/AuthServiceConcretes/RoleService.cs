using BlueBerry24.Application.Services.Interfaces.AuthServiceInterfaces;
using BlueBerry24.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity;

namespace BlueBerry24.Services.AuthAPI.Services
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<bool> AssignRoleToUserAsync(ApplicationUser user, string roleName)
        {
            if (user == null || string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentException("User and role name must be provided");
            }

            await EnsureRoleExistsAsync(roleName);

            if (!await _userManager.IsInRoleAsync(user, roleName))
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);
                return result.Succeeded;
            }

            return true;
        }

        public async Task EnsureRoleExistsAsync(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new ApplicationRole(roleName));
            }
        }

        public bool IsValidRole(string roleName)
        {
            return !string.IsNullOrEmpty(roleName) && roleName.Length <= 50;
        }
    }
}
