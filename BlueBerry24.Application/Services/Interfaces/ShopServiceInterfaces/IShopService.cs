using BlueBerry24.Application.Dtos.ShopDtos;
using BlueBerry24.Domain.Entities.ShopEntities;

namespace BlueBerry24.Application.Services.Interfaces.ShopServiceInterfaces
{
    public interface IShopService
    {
        Task<Shop> GetShopAsync(int id);
        Task<ShopDto> UpdateShopAsync(int id, UpdateShopDto shop);
    }
}
