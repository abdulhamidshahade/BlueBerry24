using Berryfy.Domain.Entities.ShopEntities;

namespace Berryfy.Domain.Repositories.ShopInterfaces
{
    public interface IShopRepository
    {
        Task<Shop> GetShopAsync(int id);
        Task<Shop> UpdateShopAsync(Shop shop);
    }
}
