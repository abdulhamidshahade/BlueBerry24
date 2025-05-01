using BlueBerry24.Application.Dtos.CouponDtos;

namespace BlueBerry24.Application.Services.Interfaces.CouponServiceInterfaces
{
    public interface IUserCouponService
    {
        Task<UserCouponDto> AddCouponToUserAsync(int userId, int couponId);
        Task<bool> DisableCouponToUser(int usreId, int couponId);
        Task<List<string>> GetCouponsByUserIdAsync(int userId);
        Task<List<string>> GetUsersByCouponIdAsync(int couponId);
        Task<bool> IsCouponUsedByUser(int userId, int couponId);
    }
}
