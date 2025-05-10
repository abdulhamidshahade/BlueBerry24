using BlueBerry24.Domain.Entities.CouponEntities;
using System.Linq.Expressions;

namespace BlueBerry24.Domain.Repositories.CouponInterfaces
{
    public interface ICouponRepository
    {
        Task<Coupon> GetByIdAsync(int id);
        Task<Coupon> GetByCodeAsync(string code);
        Task<IEnumerable<Coupon>> GetAllAsync();
        Task<Coupon> CreateAsync(Coupon coupon);
        Task<Coupon> UpdateAsync(int id, Coupon coupon);
        Task<bool> DeleteAsync(Coupon coupon);
        Task<bool> ExistsAsync(Expression<Func<Coupon, bool>> expression);
    }
}
