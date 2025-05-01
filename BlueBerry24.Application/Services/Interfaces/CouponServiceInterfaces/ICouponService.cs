using BlueBerry24.Application.Dtos.CouponDtos;

namespace BlueBerry24.Application.Services.Interfaces.CouponServiceInterfaces
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
