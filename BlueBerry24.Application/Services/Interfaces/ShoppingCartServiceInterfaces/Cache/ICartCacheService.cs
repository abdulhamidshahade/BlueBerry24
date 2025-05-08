using BlueBerry24.Application.Dtos.ShoppingCartDtos;

namespace BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces.Cache
{
    public interface ICartCacheService
    {
        Task<CartDto> GetCartAsync(int userId);
        //Task SetCartAsync(string userId, CartDto cart, TimeSpan timeSpan);
        Task<bool> DeleteCartAsync(int userId);
        Task<IEnumerable<string>> GetAllActiveCartUserIdsAsync();
    }
}
