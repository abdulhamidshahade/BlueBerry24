using BlueBerry24.Domain.Entities.Product;

namespace BlueBerry24.Domain.Repositories.ProductInterfaces
{
    public interface ICategoryRepository
    {
        Task<Category> GetByIdAsync(int id);
        Task<Category> GetByNameAsync(string name);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> CreateAsync(Category category);
        Task<Category> UpdateAsync(int id, Category category);
        Task<bool> DeleteAsync(Category category);
        Task<bool> ExistsByIdAsync(int id);
        Task<bool> ExistsByNameAsync(string name);
    }
}
