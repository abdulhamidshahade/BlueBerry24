using BlueBerry24.Domain.Entities.AuthEntities;
using BlueBerry24.Domain.Entities.CouponEntities;
namespace BlueBerry24.Domain.Repositories.CouponInterfaces
{
    public interface IUserCouponRepository
    {
        Task<UserCoupon> AddCouponToUserAsync(int userId, int couponId);
        Task<bool> DisableCouponForUserAsync(int userId, int couponId);
        Task<IReadOnlyList<Coupon>> GetCouponsByUserIdAsync(int userId);
        Task<IReadOnlyList<ApplicationUser>> GetUsersByCouponIdAsync(int couponId);
        Task<bool> IsCouponUsedByUserAsync(int userId, string couponCode);
        Task<bool> MarkCouponAsUsedAsync(int userId, int couponId, int orderId);
        Task<bool> RevertCouponUsageAsync(int userId, int couponId, int orderId);
        Task<List<int>> GetCouponIdsUsedInOrderAsync(int orderId);
    }
}
