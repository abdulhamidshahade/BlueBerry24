using BlueBerry24.Application.Dtos.ShoppingCartDtos;
using BlueBerry24.Domain.Entities.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces.Cache
{
    public interface ICartHeaderCacheService
    {
        Task<bool> ExistsByUserIdAsync(int userId);
        Task<bool> CreateCartHeaderAsync(int userId, CartHeaderDto cartHeader, TimeSpan timeSpan);
        Task<bool> DeleteCartHeaderAsync(int userId);
        Task<bool> UpdateCartHeaderAsync(int userId, CartHeaderDto cartHeader);
        Task<CartHeaderDto> GetCartHeaderAsync(int userId);
    }
}
