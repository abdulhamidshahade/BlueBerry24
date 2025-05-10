using BlueBerry24.Domain.Entities.ShopEntities;

namespace BlueBerry24.Domain.Repositories.ShopInterfaces
{
    public interface IShopRepository
    {
        Task<Shop> GetByIdAsync(int id);
        Task<Shop> GetByNameAsync(string name);
        Task<IEnumerable<Shop>> GetAllAsync();
        Task<Shop> CreateAsync(Shop shop);
        Task<Shop> UpdateAsync(int id, Shop shopDto);
        Task<bool> DeleteAsync(Shop shop);
        Task<bool> ExistsByIdAsync(int id);
        Task<bool> ExistsByNameAsync(string name);
    }
}
