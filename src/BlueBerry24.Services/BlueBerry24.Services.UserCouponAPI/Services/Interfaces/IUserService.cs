namespace BlueBerry24.Services.UserCouponAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> IsUserExistsByIdAsync(string userId);
        Task<bool> IsUserExistsByEmailAsync(string userId);
    }
}
