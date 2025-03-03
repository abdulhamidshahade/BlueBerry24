using BlueBerry24.Services.CouponAPI.Models.DTOs;

namespace BlueBerry24.Services.CouponAPI.Services.Interfaces
{
    public interface ICouponService
    {
        Task<CouponDto> GetByIdAsync(int id);
        Task<CouponDto> GetByCodeAsync(string code);
        Task<IEnumerable<CouponDto>> GetAllAsync();
        Task<CouponDto> CreateAsync(CouponDto couponDto);
        Task<CouponDto> UpdateAsync(int id, CouponDto couponDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByCodeAsync(string code);
    }
}
