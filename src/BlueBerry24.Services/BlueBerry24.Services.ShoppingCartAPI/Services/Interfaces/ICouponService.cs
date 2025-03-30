using BlueBerry24.Services.ShoppingCartAPI.Models.DTOs;

namespace BlueBerry24.Services.ShoppingCartAPI.Services.Interfaces
{
    public interface ICouponService
    {
        Task<bool> IsUsedByUserAsync(string userId, string couponCode);
        //Task<bool> IsAvailableAsync(string couponCode);


        Task<decimal> RedeemCouponAsync(string userId, string headerId, string couponCode, decimal total);
        Task<CouponDto> GetCouponByNameAsync(string userId, string couponCode);
    }
}
