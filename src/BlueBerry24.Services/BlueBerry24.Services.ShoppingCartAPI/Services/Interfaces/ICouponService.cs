namespace BlueBerry24.Services.ShoppingCartAPI.Services.Interfaces
{
    public interface ICouponService
    {
        Task<bool> IsUsedByUserAsync(string userId);
        Task<bool> IsAvailableAsync(string userId);
    }
}
