using BlueBerry24.Services.ShoppingCartAPI.Models.DTOs;

namespace BlueBerry24.Services.ShoppingCartAPI.Services.Interfaces
{
    public interface ICartCacheService
    {
        Task<CartDto> GetCartAsync(string userId);
        Task SetCartAsync(string userId, CartDto cart);
    }
}
