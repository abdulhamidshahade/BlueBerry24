using BlueBerry24.Domain.Entities.Product;

namespace BlueBerry24.Domain.Repositories.ProductInterfaces
{
    public interface ICategoryRepository
    {
        Task<Category> GetByIdAsync(string id);
        Task<Category> GetByNameAsync(string name);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> CreateAsync(Category category);
        Task<Category> UpdateAsync(string id, Category category);
        Task<bool> DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<bool> ExistsByNameAsync(string name);
    }
}
