using Berryfy.Domain.Entities.ProductEntities;

namespace Berryfy.Domain.Repositories.ProductInterfaces
{
    public interface IProductCategoryRepository
    {
        Task<bool> AddProductCategoryAsync(Product Product, List<int> categories);
        Task<bool> UpdateProductCategoryAsync(Product product, List<int> categories);
        Task<List<Category>> GetCategoriesByProuductId(int productId);
        Task<bool> RemoveCategoriesByProductId(int productId);
    }
}