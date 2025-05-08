using BlueBerry24.Domain.Entities.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Domain.Repositories.ShoppingCartInterfaces
{
    public interface ICartRepository
    {
        Task<ShoppingCart> UpdateCartAsync(int userId, bool isActive);
        Task<ShoppingCart> CreateCartAsync(int userId, int headerId);
        Task<ShoppingCart> GetCartAsync(int userId);
        Task<List<ShoppingCart>> GetCartsAsync();
        Task<bool> DeleteCartAsync(int userId);

    }
}
