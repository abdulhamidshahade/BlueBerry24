﻿using BlueBerry24.Domain.Entities.ShoppingCartEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Domain.Repositories.ShoppingCartInterfaces
{
    public interface ICartItemRepository
    {
        //Task<CartItem> AddItemAsync(CartItem item);
        //Task<bool> RemoveItemAsync(CartItem item);
        //Task<CartItem> UpdateItemCountAsync(CartItem item, int newCount);


        //Task<bool> IncreaseItemAsync(CartItem item);
        //Task<bool> DecreaseItemAsync(CartItem item);


        //Task<bool> ExistsByUserIdAsync(int userId, int headerId);


        Task<bool> LoadItemsToPersistenceStorage(List<CartItem> items, int cartId);
        Task<List<CartItem>> GetItemsFromPersistenceStorage(int cartId);
        Task<bool> RemoveItemsFromPersistenceStorage(int cartId);
    }
}
