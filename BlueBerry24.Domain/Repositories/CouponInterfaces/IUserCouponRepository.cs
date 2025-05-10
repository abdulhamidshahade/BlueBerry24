using BlueBerry24.Domain.Entities.AuthEntities;
using BlueBerry24.Domain.Entities.CouponEntities;
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
        Task<IReadOnlyList<ApplicationUser>> GetUsersByCouponIdAsync(int couponId);
        Task<bool> IsCouponUsedByUserAsync(int userId, int couponId);
    }
}
