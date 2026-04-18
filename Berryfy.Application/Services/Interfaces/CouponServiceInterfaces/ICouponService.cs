using Berryfy.Application.Dtos.CouponDtos;

namespace Berryfy.Application.Services.Interfaces.CouponServiceInterfaces
{
    public interface ICouponService
    {
        Task<CouponDto> GetByIdAsync(int id);
        Task<CouponDto> GetByCodeAsync(string code);
        Task<IEnumerable<CouponDto>> GetAllAsync();
        Task<CouponDto> CreateAsync(CreateCouponDto couponDto);
        Task<CouponDto> UpdateAsync(int id, UpdateCouponDto couponDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
        Task<bool> ExistsByCodeAsync(string code);
    }
}
