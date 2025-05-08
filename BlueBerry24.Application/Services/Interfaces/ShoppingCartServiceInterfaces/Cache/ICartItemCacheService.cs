using BlueBerry24.Application.Dtos.ShoppingCartDtos;
using BlueBerry24.Domain.Entities.ShoppingCart;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces.Cache
{
    public interface ICartItemCacheService
    {
        Task<bool> AddItemAsync(CartItemDto item, int userId);
        Task<bool> RemoveItemAsync(CartItemDto item, int userId);
        Task<bool> UpdateItemCountAsync(CartItemDto item, int userId, int newCount);

        Task<bool> IncreaseItemAsync(CartItemDto item, int userId);
        Task<bool> DecreaseItemAsync(CartItemDto item, int userId);
        Task<List<CartItemDto>> GetAllItems(int userId);
        Task<bool> DeleteAllItems(int userId);
    }
}
