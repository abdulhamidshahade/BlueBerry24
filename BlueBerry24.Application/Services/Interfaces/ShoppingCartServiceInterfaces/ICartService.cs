using BlueBerry24.Application.Dtos.ShoppingCartDtos;
using BlueBerry24.Domain.Entities.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync(int userId);
        Task<bool> DeleteCartAsync(int userId);
        Task<CartDto> UpdateCartAsync(int userId, bool isActive);
        Task<List<CartDto>> GetCartsAsync();
        Task<bool> IsCartAsync(int userId);
        Task<CartDto> CreateCartAsync(int userId, int headerId);
    }
}
