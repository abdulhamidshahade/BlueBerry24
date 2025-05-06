using BlueBerry24.Application.Dtos.ShoppingCartDtos;
using BlueBerry24.Domain.Entities.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces
{
    interface ICartHeaderService
    {
        Task<bool> ExistsByIdAsync(int id);
        Task<CartHeaderDto> CreateCartAsync(CartHeaderDto header);
        Task<bool> DeleteCartHeaderAsync(int id);
        Task<CartHeaderDto> UpdateCartHeaderAsync(int id, CartHeaderDto header);
    }
}
