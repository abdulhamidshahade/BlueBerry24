using BlueBerry24.Application.Dtos.AuthDtos;
namespace BlueBerry24.Application.Services.Interfaces.AuthServiceInterfaces
{
    public interface IRoleManagementService
    {
        Task<bool> CreateRoleAsync(string roleName);
        Task<bool> DeleteRoleAsync(string roleName);
        Task<bool> AssignRoleToUserAsync(int userId, string roleName);
        Task<bool> RemoveRoleFromUserAsync(int userId, string roleName);
        Task<List<string>> GetUserRolesAsync(int userId);
        Task<List<ApplicationRoleDto>> GetAllRolesAsync();
        Task<List<ApplicationUserDto>> GetUsersInRoleAsync(string roleName);
        Task<bool> IsUserInRoleAsync(int userId, string roleName);
        Task InitializeDefaultRolesAsync();
        Task<List<ApplicationUserWithRolesDto>> GetAllUsersAsync();
        Task<ApplicationUserWithRolesDto> GetUserByIdAsync(int userId);
        Task<RoleStatsDto> GetRoleStatsAsync();
        Task<bool> UpdateRoleAsync(string oldRoleName, string newRoleName);
        Task<BulkAssignmentResultDto> BulkAssignRoleAsync(List<int> userIds, string roleName);
    }
}
