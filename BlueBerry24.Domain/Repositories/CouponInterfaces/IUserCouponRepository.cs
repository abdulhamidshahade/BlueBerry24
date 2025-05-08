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
        Task<bool> DisableCouponToUserAsync(int userId, int couponId);
        Task<List<string>> GetCouponsByUserIdAsync(int userId);
        Task<List<string>> GetUsersByCouponIdAsync(int couponId);
        Task<bool> IsCouponUsedByUser(int userId, int couponId);
    }
}
