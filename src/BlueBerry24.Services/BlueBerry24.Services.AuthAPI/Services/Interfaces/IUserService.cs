namespace BlueBerry24.Services.AuthAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> IsUserExistsByIdAsync(string userId);
        Task<bool> IsUserExistsByEmailAsync(string emailAddress);
    }
}
