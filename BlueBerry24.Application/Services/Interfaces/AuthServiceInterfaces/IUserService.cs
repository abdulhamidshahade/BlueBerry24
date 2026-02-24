using BlueBerry24.Application.Dtos.AuthDtos;

namespace BlueBerry24.Application.Services.Interfaces.AuthServiceInterfaces
{
    public interface IUserService
    {
        Task<bool> IsUserExistsByIdAsync(int userId);
        Task<bool> IsUserExistsByEmailAsync(string emailAddress);
        Task<List<ApplicationUserDto>> GetAllUsers();
        Task<ApplicationUserDto> GetUserById(int id);
        Task<ApplicationUserDto> GetUserByEmail(string email);
        Task<bool> LockUserAccountAsync(int userId, DateTime? lockoutEnd = null);
        Task<bool> UnlockUserAccountAsync(int userId);
        Task<bool> ResetUserPasswordAsync(int userId, string newPassword);
        Task<bool> VerifyUserEmailAsync(int userId);
        Task<bool> UpdateUserAsync(int userId, UpdateUserDto updateUserDto);
        Task<ApplicationUserDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> IsUsernameTaken(string username);
    }
}
