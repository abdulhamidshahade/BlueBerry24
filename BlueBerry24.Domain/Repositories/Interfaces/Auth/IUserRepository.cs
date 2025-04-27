namespace BlueBerry24.Domain.Repositories.Interfaces.Auth
{
    public interface IUserRepository
    {
        Task<bool> IsUserExistsByIdAsync(string userId);
        Task<bool> IsUserExistsByEmailAsync(string emailAddress);
    }
}
