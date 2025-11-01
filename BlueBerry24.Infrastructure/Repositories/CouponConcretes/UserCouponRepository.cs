using BlueBerry24.Domain.Entities.AuthEntities;
using BlueBerry24.Domain.Entities.CouponEntities;
using BlueBerry24.Domain.Repositories;
using BlueBerry24.Domain.Repositories.CouponInterfaces;
using BlueBerry24.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry24.Infrastructure.Repositories.CouponConcretes
{
    public class UserCouponRepository : IUserCouponRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unifOfWork;

        public UserCouponRepository(ApplicationDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unifOfWork = unitOfWork;
        }

        public async Task<UserCoupon> AddCouponToUserAsync(int userId, int couponId)
        {
            var userCoupon = new UserCoupon
            {
                UserId = userId,
                CouponId = couponId,
                IsUsed = false,
            };

            await _context.UserCoupons.AddAsync(userCoupon);

            if(await _unifOfWork.SaveDbChangesAsync())
            {
                return userCoupon;
            }

            return null;
        }

        public async Task<UserCoupon> AddUserToCouponAsync(int userId, int couponId)
        {
            var userCoupon = new UserCoupon
            {
                UserId = userId,
                CouponId = couponId,
                IsUsed = false
            };

            await _context.UserCoupons.AddAsync(userCoupon);
            await _unifOfWork.SaveDbChangesAsync();

            return userCoupon;
        }

        public async Task<bool> DisableCouponForUserAsync(int userId, int couponId)
        {
            var userCouponModel = await _context.UserCoupons.Where(i => i.UserId == userId && i.CouponId == couponId)
                .FirstOrDefaultAsync();

            userCouponModel.IsUsed = true;
            return await _unifOfWork.SaveDbChangesAsync();
        }

        public async Task<IReadOnlyList<Coupon>> GetCouponsByUserIdAsync(int userId)
        {
            var coupons = await _context.UserCoupons.Where(i => i.UserId == userId)
                .Select(c => c.Coupon)
                .ToListAsync();

            return coupons;
        }

        public async Task<IReadOnlyList<ApplicationUser>> GetUsersByCouponIdAsync(int couponId)
        {
            var users = await _context.UserCoupons.Where(c => c.CouponId == couponId)
                .Select(u => u.User)
                .ToListAsync();

            return users;
        }

        public async Task<bool> IsCouponUsedByUserAsync(int userId, string couponCode)
        {
            var coupon = await _context.UserCoupons.Where(i => i.UserId == userId && i.Coupon.Code == couponCode)
                .FirstOrDefaultAsync();

            if(coupon == null)
            {
                return false;
            }

            return coupon.IsUsed;
        }
    }
}