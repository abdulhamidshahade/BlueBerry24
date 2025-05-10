using BlueBerry24.Domain.Entities.ShoppingCartEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Domain.Repositories.ShoppingCartInterfaces
{
    public interface ICartHeaderRepository
    {
        Task<bool> ExistsByIdAsync(int userId);
        Task<CartHeader> CreateCartAsync(int userId);
        Task<bool> DeleteCartHeaderAsync(int userId);
        Task<CartHeader> UpdateCartHeaderAsync(int userId, CartHeader header);
        Task<CartHeader> GetCartHeaderAsync(int userId);
    }
}
