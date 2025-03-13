using BlueBerry24.Services.ProductAPI.Models;
using BlueBerry24.Services.ProductAPI.Models.DTOs.ProductDtos;

namespace BlueBerry24.Services.ProductAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> GetByIdAsync(string id);
        Task<ProductDto> GetByNameAsync(string name);
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto> CreateAsync(CreateProductDto productDto, List<string> categories);
        Task<ProductDto> UpdateAsync(string id, UpdateProductDto productDto, List<string> categories);
        Task<bool> DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<bool> ExistsByNameAsync(string name);
    }
}
