using BlueBerry24.Application.Dtos.AuthDtos;
using BlueBerry24.Application.Dtos.CouponDtos;

namespace BlueBerry24.Application.Services.Interfaces.CouponServiceInterfaces
{
    public interface IUserCouponService
    {
        Task<UserCouponDto> AddCouponToUserAsync(int userId, int couponId);
        Task<bool> DisableCouponToUser(int usreId, int couponId);
        Task<List<CouponDto>> GetCouponsByUserIdAsync(int userId);
        Task<List<ApplicationUserDto>> GetUsersByCouponIdAsync(int couponId);
        Task<bool> IsCouponUsedByUser(int userId, string couponCode);
    }
}
