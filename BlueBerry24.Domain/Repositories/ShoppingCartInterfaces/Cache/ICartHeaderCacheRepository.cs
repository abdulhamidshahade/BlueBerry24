using BlueBerry24.Domain.Entities.ShoppingCart;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Domain.Repositories.ShoppingCartInterfaces.Cache
{
    public interface ICartHeaderCacheRepository
    {
        Task<bool> ExistsByUserIdAsync(string key);
        Task<bool> CreateCartHeaderAsync(string key, CartHeader cartHeader, TimeSpan timeSpan, ITransaction? transaction = null);
        Task<bool> DeleteCartHeaderAsync(int userId, string key, ITransaction transaction);
        Task<bool> UpdateCartHeaderAsync(string key, CartHeader cartHeader);
        Task<CartHeader> GetCartHeaderAsync(string key);
    }
}
