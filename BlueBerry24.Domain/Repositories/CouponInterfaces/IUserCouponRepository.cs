using BlueBerry24.Domain.Entities.Coupon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Domain.Repositories.CouponInterfaces
{
    public interface IUserCouponRepository
    {
        Task<UserCoupon> AddCouponToUserAsync(int userId, int couponId);
        Task<bool> DisableCouponForUserAsync(int userId, int couponId);
        Task<IReadOnlyList<Coupon>> GetCouponsByUserIdAsync(int userId);
        Task<IReadOnlyList<User>> GetUsersByCouponIdAsync(int couponId);
        Task<bool> IsCouponUsedByUserAsync(int userId, int couponId);
    }
}
