using BlueBerry24.Services.UserCouponAPI.Models.DTOs;

namespace BlueBerry24.Services.UserCouponAPI.Services.Interfaces
{
    public interface IUserCouponService
    {
        Task<UserCouponDto> AddCouponToUserAsync(string userId, string couponId);
        Task<bool> DisableUserCouponAsync(string usreId, string couponId);
        Task<IReadOnlyCollection<string>> GetCouponsByUserIdAsync(string userId);
        Task<IReadOnlyCollection<string>> GetUsersByCouponIdAsync(string couponId);
        Task<bool> IsCouponUsedByUser(string userId, string couponId);
    }
}
