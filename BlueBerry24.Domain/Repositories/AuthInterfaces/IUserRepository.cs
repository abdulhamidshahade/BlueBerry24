namespace BlueBerry24.Domain.Repositories.AuthInterfaces
{
    public interface IUserRepository
    {
        Task<bool> IsUserExistsByIdAsync(string userId);
        Task<bool> IsUserExistsByEmailAsync(string emailAddress);
    }
}
