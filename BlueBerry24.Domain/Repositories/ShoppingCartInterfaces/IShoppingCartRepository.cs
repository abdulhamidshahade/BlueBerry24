using BlueBerry24.Domain.Entities.ShoppingCart;

namespace BlueBerry24.Domain.Repositories.ShoppingCartInterfaces
{
    public interface IShoppingCartRepository
    {
        Task<bool> ExistsByHeaderIdAsync(string userId,string headerId);
        Task<bool> ExistsByUserIdAsync(string userId, string headerId);
        Task<CartHeader> CreateCartAsync(string userId);
        Task<bool> AddItemAsync(string userId, string headerId, CartItem item);
        Task<bool> RemoveItemAsync(string userId, CartItem item);
        Task<bool> UpdateItemCountAsync(string userId, string headerId, string itemId, int newCount);
        Task<bool> DeleteShoppingCartAsync(string userId, string headerId);
        Task<bool> UpdateCartHeaderAsync(string userId, string headerId, CartHeader header);
        Task<bool> IncreaseItemAsync(string userId, CartItem item);
        Task<bool> DecreaseItemAsync(string userId, CartItem item);
    }
}
