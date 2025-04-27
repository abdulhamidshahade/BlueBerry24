using BlueBerry24.Domain.Entities.Product;

namespace BlueBerry24.Domain.Repositories.ProductInterfaces
{
    public interface IProductCategoryRepository
    {
        Task<bool> AddProductCategoryAsync(Product Product, List<string> categories);
        Task<bool> UpdateProductCategoryAsync(Product product, List<string> categories);
    }
}