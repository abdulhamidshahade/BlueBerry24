using Berryfy.Application.Dtos.ShopDtos;
using Berryfy.Domain.Entities.ShopEntities;

namespace Berryfy.Application.Services.Interfaces.ShopServiceInterfaces
{
    public interface IShopService
    {
        Task<Shop> GetShopAsync(int id);
        Task<ShopDto> UpdateShopAsync(int id, UpdateShopDto shop);
    }
}
