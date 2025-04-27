using BlueBerry24.Domain.Entities.Product;

namespace BlueBerry24.Domain.Repositories.ProductInterfaces
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(string id);
        Task<Product> GetByNameAsync(string name);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> CreateAsync(Product product, List<string> categories);
        Task<Product> UpdateAsync(string id, Product product, List<string> categories);
        Task<bool> DeleteAsync(string id);
        Task<bool> ExistsByIdAsync(string id);
        Task<bool> ExistsByNameAsync(string name);
        Task<bool> ExistsByShopIdAsync(string productId, string shopId);
    }
}
