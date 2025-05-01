using BlueBerry24.Domain.Entities.Coupon;
using BlueBerry24.Domain.Repositories;
using BlueBerry24.Domain.Repositories.CouponInterfaces;
using BlueBerry24.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Infrastructure.Repositories.CouponConcretes
{
    class UserCouponRepository : IUserCouponRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unifOfWork;

        public UserCouponRepository(ApplicationDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unifOfWork = unitOfWork;
        }

        public Task<UserCoupon> AddCouponToUserAsync(int userId, int couponId)
        {
            throw new NotImplementedException();
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

        public async Task<bool> DisableCouponToUserAsync(int userId, int couponId)
        {
            var userCouponModel = await _context.UserCoupons.Where(i => i.UserId == userId && i.CouponId == couponId)
                .FirstOrDefaultAsync();

            userCouponModel.IsUsed = true;
            return await _unifOfWork.SaveDbChangesAsync();
        }

        public async Task<List<string>> GetCouponsByUserIdAsync(int userId)
        {
            var coupons = await _context.UserCoupons.Where(i => i.UserId == userId)
                .Select(c => c.Coupon)
                .Select(c => c.ToString())
                .ToListAsync();

            return coupons;
        }

        public async Task<List<string>> GetUsersByCouponIdAsync(int couponId)
        {
            var users = await _context.UserCoupons.Where(c => c.CouponId == couponId)
                .Select(u => u.User)
                .Select(u => u.ToString())
                .ToListAsync();

            return users;
        }

        public async Task<bool> IsCouponUsedByUser(int userId, int couponId)
        {
            var coupon = await _context.UserCoupons.Where(i => i.UserId == userId && couponId == couponId)
                .FirstOrDefaultAsync();

            return coupon.IsUsed;
        }
    }
}
