using BlueBerry24.Domain.Entities.Coupon;

namespace BlueBerry24.Domain.Repositories.CouponInterfaces
{
    public interface ICouponRepository
    {
        Task<Coupon> GetByIdAsync(string id);
        Task<Coupon> GetByCodeAsync(string code);
        Task<IEnumerable<Coupon>> GetAllAsync();
        Task<Coupon> CreateAsync(Coupon coupon);
        Task<Coupon> UpdateAsync(string id, Coupon coupon);
        Task<bool> DeleteAsync(string id);
        Task<bool> ExistsByIdAsync(string id);
        Task<bool> ExistsByCodeAsync(string code);
    }
}
