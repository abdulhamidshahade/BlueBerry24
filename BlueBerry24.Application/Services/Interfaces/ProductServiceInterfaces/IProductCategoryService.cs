using BlueBerry24.Application.Dtos.ProductDtos;

namespace BlueBerry24.Application.Services.Interfaces.ProductServiceInterfaces
{
    public interface IProductCategoryService
    {
        Task<bool> AddProductCategoryAsync(ProductDto Product, List<int> categories);
        Task<bool> UpdateProductCategoryAsync(ProductDto product, List<int> categories);
    }
}
