﻿using BlueBerry24.Domain.Entities.ShoppingCartEntities;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Domain.Repositories.ShoppingCartInterfaces.Cache
{
    public interface ICartItemCacheRepository
    {
        Task<bool> AddItemAsync(CartItem item, string key, ITransaction? transaction = null);
        Task<bool> RemoveItemAsync(CartItem item, string key);
        Task<bool> UpdateItemCountAsync(CartItem item, string key, int newCount);

        Task<bool> IncreaseItemAsync(CartItem item, string key);
        Task<bool> DecreaseItemAsync(CartItem item, string key, ITransaction? transaction = null);
        Task<List<CartItem>> GetAllItems(string key);
        Task<bool> DeleteAllItems(string key, ITransaction? transaction = null);
    }
}
