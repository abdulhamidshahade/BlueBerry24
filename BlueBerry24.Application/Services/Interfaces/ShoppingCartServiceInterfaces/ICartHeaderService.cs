using BlueBerry24.Application.Dtos.ShoppingCartDtos;
using BlueBerry24.Domain.Entities.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces
{
    public interface ICartHeaderService
    {
        Task<bool> ExistsByIdAsync(int userId);
        Task<CartHeaderDto> CreateCartHeaderAsync(int userId);
        Task<bool> DeleteCartHeaderAsync(int userId);
        Task<CartHeaderDto> UpdateCartHeaderAsync(int userId, CartHeaderDto header);
        Task<CartHeaderDto> GetCartHeaderAsync(int userId);
    }
}
