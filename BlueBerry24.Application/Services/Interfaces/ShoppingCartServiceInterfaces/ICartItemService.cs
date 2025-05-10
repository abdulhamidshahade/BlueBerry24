using BlueBerry24.Application.Dtos.ShoppingCartDtos;
using BlueBerry24.Domain.Entities.ShoppingCartEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces
{
    public interface ICartItemService
    {
        //Task<CartItemDto> AddItemAsync(CartItemDto item);
        //Task<bool> RemoveItemAsync(CartItemDto item);
        //Task<CartItemDto> UpdateItemCountAsync(CartItemDto item, int newCount);

        //Task<bool> ExistsByUserIdAsync(int userId, int headerId);
        //Task<bool> IncreaseItemAsync(CartItemDto item);
        //Task<bool> DecreaseItemAsync(CartItemDto item);

        Task<bool> LoadItemsToPersistenceStorage(List<CartItem> items, int cartId);
        Task<List<CartItemDto>> GetItemsFromPersistenceStorage(int cartId);
        Task<bool> RemoveItemsFromPersistenceStorage(int cartId);

    }
}
