namespace BlueBerry24.Domain.Repositories.AuthInterfaces
{
    public interface IUserRepository
    {
        Task<bool> IsUserExistsByIdAsync(int userId);
        Task<bool> IsUserExistsByEmailAsync(string emailAddress);
    }
}
