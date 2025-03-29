namespace BlueBerry24.Services.ShoppingCartAPI.Services.Interfaces
{
    public interface ICouponService
    {
        Task<bool> IsUsedByUserAsync(string userId, string couponCode);
        //Task<bool> IsAvailableAsync(string couponCode);
    }
}
