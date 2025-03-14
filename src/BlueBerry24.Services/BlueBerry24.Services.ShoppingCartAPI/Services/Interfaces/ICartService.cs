using BlueBerry24.Services.ShoppingCartAPI.Models.DTOs;

namespace BlueBerry24.Services.ShoppingCartAPI.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCartByHeaderIdAsync(string userId,string headerId);
        Task<CartDto> GetShoppingCartByUserIdAsync(string userId);
        Task<bool> ExistsByHeaderIdAsync(string userId,string headerId);
        Task<bool> ExistsByUserIdAsync(string userId);

        Task<CartDto> CreateCartAsync(string userId);
        Task<CartDto> AddItemAsync(string userId, string headerId, CartItemDto itemDto);
        Task<bool> RemoveItemAsync(string userId, string headerId, string itemId);
        Task<CartDto> UpdateItemCountAsync(string userId, string headerId, string itemId, int newCount);
        Task<CartDto> DeleteShoppingCartAsync(string userId, string headerId);

        Task<CartDto> ApplyCouponAsync(string userId, string headerId, string couponCode);
        Task<CartDto> UpdateCartHeaderAsync(string userId, string headerId, CartHeaderDto headerDto);

        Task<bool> ValidateCouponAsync(string userId, string couponCode);
        Task<bool> ValidateShopOwner(string userId);
    }
}
