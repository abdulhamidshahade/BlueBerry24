using BlueBerry24.Application.Dtos.ShoppingCartDtos;

namespace BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces
{
    public interface ICartCacheService
    {
        Task<CartDto> GetCartAsync(string userId);
        Task SetCartAsync(string userId, CartDto cart, TimeSpan timeSpan);
        Task DeleteCartAsync(string userId);
        Task<IEnumerable<string>> GetAllActiveCartUserIdsAsync();
    }
}
