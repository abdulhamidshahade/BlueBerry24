using BlueBerry24.Domain.Entities.Coupon;
using BlueBerry24.Domain.Repositories.CouponInterfaces;
using BlueBerry24.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace BlueBerry24.Infrastructure.Repositories.CouponConcretes
{
    class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _context;
        public CouponRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Coupon>> GetAllAsync()
        {
            var coupons = await _context.Coupons.ToListAsync();
            return coupons;
        }

        public async Task<Coupon> GetByCodeAsync(string code)
        {
            var coupon = await _context.Coupons.Where(c => c.Code == code).FirstOrDefaultAsync();
            return coupon;
        }

        public async Task<Coupon> GetByIdAsync(int id)
        {
            var coupon = await _context.Coupons.FindAsync(id);
            return coupon;
        }

        public async Task<Coupon> GetAsync(Expression<Func<Coupon, bool>> expression)
        {
            return await _context.Coupons.Where(expression).FirstOrDefaultAsync();
        }

        public async Task<Coupon> CreateAsync(Coupon coupon)
        {
            await _context.Coupons.AddAsync(coupon);
            await _context.SaveChangesAsync();

            return coupon;
        }

        public async Task<Coupon> UpdateAsync(int id, Coupon coupon)
        {
            var couponModel = await _context.Coupons.FindAsync(id); //reduces the query cost.

            if (couponModel == null) return null;

            couponModel.MinimumAmount = coupon.MinimumAmount;
            couponModel.DiscountAmount = coupon.DiscountAmount;
            couponModel.IsActive = coupon.IsActive;

            _context.Coupons.Update(couponModel);
            await _context.SaveChangesAsync();

            return couponModel;
        }

        public async Task<bool> DeleteAsync(Coupon coupon)
        {
            _context.Coupons.Remove(coupon);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(Expression<Func<Coupon, bool>> expression)
        {
            return await _context.Coupons.AnyAsync(expression);
        }
    }
}
