using BlueBerry24.Services.AuthAPI.Models;

namespace BlueBerry24.Services.AuthAPI.Services.Interfaces
{
    public interface IRoleService
    {
        Task EnsureRoleExistsAsync(string roleName);
        Task<bool> AssignRoleToUserAsync(ApplicationUser user, string roleName);
        bool IsValidRole(string roleName);
    }
}
