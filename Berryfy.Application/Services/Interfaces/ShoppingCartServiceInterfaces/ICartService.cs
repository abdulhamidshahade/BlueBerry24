using Berryfy.Application.Dtos.ShoppingCartDtos;
using Berryfy.Domain.Constants;

namespace Berryfy.Application.Services.Interfaces.ShoppingCartServiceInterfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCartByUserIdAsync(int userId, CartStatus? status = CartStatus.Active);
        Task<CartDto> GetCartBySessionIdAsync(string sessionId, CartStatus? status = CartStatus.Active);
        Task<CartDto> GetCartByIdAsync(int cartId, CartStatus status);
        Task<CartItemDto> GetItemAsync(int cartId, int productId);
        Task<CartDto> CreateCartAsync(int? userId, string? sessionId);
        Task<CartDto?> AddItemAsync(int cartId, int? userId, string? sessionId, int productId, int quantity);
        Task<CartDto> UpdateItemQuantityAsync(int cartId, int? userId, string? sessionId, int productId, int quantity);
        Task<bool> RemoveItemAsync(int cartId, int? userId, string? sessionId, int productId);
        Task<bool> ClearCartAsync(int cartId, int? userId, string? sessionId);
        Task<bool> CompleteCartAsync(int cartId, int? userId);
        Task<bool> ConvertCartAsync(int cartId);
        Task<bool> UpdateCartStatusAsync(int cartId, CartStatus status);
        Task<bool> ReactivateCartAsync(int cartId, int orderId);
        Task<bool> HandleAbandonedCartAsync(int cartId);
        Task<int> CleanupExpiredCartsAsync();
        Task<CartDto> RefreshCartAsync(int cartId);
        Task<CartDto> ApplyCouponAsync(int cartId, int? userId, string couponCode);
        Task<CartDto> RemoveCouponAsync(int cartId, int? userId, string? sessionId, int couponId);
        Task MergeCartAsync(int userId, string sessionId);
        
    }
}
