using BlueBerry24.Services.CouponAPI.Models.DTOs;

namespace BlueBerry24.Services.CouponAPI.Services.Interfaces
{
    public interface ICouponService
    {
        Task<CouponDto> GetByIdAsync(string id);
        Task<CouponDto> GetByCodeAsync(string code);
        Task<IEnumerable<CouponDto>> GetAllAsync();
        Task<CouponDto> CreateAsync(CreateCouponDto couponDto);
        Task<CouponDto> UpdateAsync(string id, CouponDto couponDto);
        Task<bool> DeleteAsync(string id);
        Task<bool> ExistsByIdAsync(string id);
        Task<bool> ExistsByCodeAsync(string code);
    }
}
