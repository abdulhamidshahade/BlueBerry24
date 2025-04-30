namespace BlueBerry24.Application.Services.Interfaces.AuthServiceInterfaces
{
    public interface IUserService
    {
        Task<bool> IsUserExistsByIdAsync(string userId);
        Task<bool> IsUserExistsByEmailAsync(string emailAddress);
    }
}
