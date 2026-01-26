using BlueBerry24.Application.Dtos.WishlistDtos;

namespace BlueBerry24.Application.Services.Interfaces.WishlistServiceInterfaces
{
    public interface IWishlistService
    {
        Task<WishlistDto> GetByIdAsync(int id);
        Task<WishlistDto> GetUserDefaultWishlistAsync(int userId);
        Task<IEnumerable<WishlistDto>> GetUserWishlistsAsync(int userId);
        Task<WishlistDto> CreateAsync(int userId, CreateWishlistDto createWishlistDto);
        Task<WishlistDto> UpdateAsync(int id, UpdateWishlistDto updateWishlistDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

        Task<WishlistItemDto> AddItemAsync(int userId, AddToWishlistDto addToWishlistDto);
        Task<WishlistItemDto> UpdateItemAsync(int wishlistId, int productId, UpdateWishlistItemDto updateItemDto);
        Task<bool> RemoveItemAsync(int wishlistId, int productId);
        Task<bool> IsProductInWishlistAsync(int userId, int productId);
        Task<IEnumerable<WishlistItemDto>> GetWishlistItemsAsync(int wishlistId);


        Task<bool> AddMultipleItemsAsync(int userId, int wishlistId, List<int> productIds);
        Task<bool> RemoveMultipleItemsAsync(int wishlistId, List<int> productIds);
        Task<bool> MoveItemsToWishlistAsync(int fromWishlistId, int toWishlistId, List<int> productIds);
        Task<bool> ClearWishlistAsync(int wishlistId);

        Task<WishlistSummaryDto> GetUserSummaryAsync(int userId);
        Task<bool> ShareWishlistAsync(int wishlistId, bool isPublic);
        Task<WishlistDto> DuplicateWishlistAsync(int wishlistId, string newName);

        Task<IEnumerable<WishlistDto>> GetAllWishlistsAsync();
        Task<GlobalWishlistStatsDto> GetGlobalStatsAsync();
    }
}
