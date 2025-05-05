using BlueBerry24.Domain.Entities.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Domain.Repositories.ShoppingCartInterfaces
{
    public interface ICartHeaderRepository
    {
        Task<bool> ExistsByIdAsync(int id);
        Task<CartHeader> CreateCartAsync(int userId);
        Task<bool> DeleteCartHeaderAsync(int id);
        Task<CartHeader> UpdateCartHeaderAsync(int id, CartHeader header);
    }
}
