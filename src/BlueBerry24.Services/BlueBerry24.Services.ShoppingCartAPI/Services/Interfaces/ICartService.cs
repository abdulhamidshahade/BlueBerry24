using BlueBerry24.Services.ShoppingCartAPI.Models;
using BlueBerry24.Services.ShoppingCartAPI.Models.DTOs;

namespace BlueBerry24.Services.ShoppingCartAPI.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCartByHeaderIdAsync(string userId,string headerId);
        Task<CartDto> GetCartByUserIdAsync(string userId);
        Task<bool> ExistsByHeaderIdAsync(string userId,string headerId);
        Task<bool> ExistsByUserIdAsync(string userId, string headerId);

        Task<CartHeader> CreateCartAsync(string userId);
        Task<bool> AddItemAsync(string userId, string headerId, CartItemDto itemDto);
        Task<bool> RemoveItemAsync(string userId, string headerId, string productId);
        Task<bool> UpdateItemCountAsync(string userId, string headerId, string itemId, int newCount);
        Task<bool> DeleteShoppingCartAsync(string userId, string headerId);

        Task<bool> UpdateCartHeaderAsync(string userId, string headerId, CartHeaderDto headerDto);


        Task<bool> RedeemCouponAsync(string userId, string headerId, string couponCode);
        Task<CouponDto> GetCouponByNameAsync(string userId, string couponCode);
    }
}
