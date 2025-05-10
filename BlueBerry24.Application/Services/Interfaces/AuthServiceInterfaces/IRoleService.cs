

using BlueBerry24.Domain.Entities.AuthEntities;

namespace BlueBerry24.Application.Services.Interfaces.AuthServiceInterfaces
{
    public interface IRoleService
    {
        Task EnsureRoleExistsAsync(string roleName);
        Task<bool> AssignRoleToUserAsync(ApplicationUser user, string roleName);
        bool IsValidRole(string roleName);
    }
}
