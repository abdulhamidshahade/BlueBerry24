using BlueBerry24.Domain.Entities.ShopEntities;

namespace BlueBerry24.Domain.Repositories.ShopInterfaces
{
    public interface IShopRepository
    {
        Task<Shop> GetShopAsync(int id);
        Task<Shop> UpdateShopAsync(Shop shop);
    }
}
