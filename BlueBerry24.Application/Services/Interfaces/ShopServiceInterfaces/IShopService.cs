using BlueBerry24.Application.Dtos.ShopDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Services.Interfaces.ShopServiceInterfaces
{
    public interface IShopService
    {
        Task<ShopDto> GetByIdAsync(int id);
        Task<ShopDto> GetByNameAsync(string name);
        Task<IEnumerable<ShopDto>> GetAllAsync();
        Task<ShopDto> CreateAsync(CreateShopDto shopDto);
        Task<ShopDto> UpdateAsync(int id, UpdateShopDto shopDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
        Task<bool> ExistsByNameAsync(string name);
    }
}
