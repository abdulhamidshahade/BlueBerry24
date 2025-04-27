using BlueBerry24.Domain.Entities.Shop;

namespace BlueBerry24.Domain.Repositories.ShopInterfaces
{
    public interface IShopRepository
    {
        Task<Shop> GetByIdAsync(string id);
        Task<Shop> GetByNameAsync(string name);
        Task<IEnumerable<Shop>> GetAllAsync();
        Task<Shop> CreateAsync(Shop shop);
        Task<Shop> UpdateAsync(string id, Shop shopDto);
        Task<bool> DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<bool> ExistsByNameAsync(string name);
    }
}
