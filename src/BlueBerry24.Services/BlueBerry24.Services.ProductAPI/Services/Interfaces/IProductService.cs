using BlueBerry24.Services.ProductAPI.Models;
using BlueBerry24.Services.ProductAPI.Models.DTOs.ProductDtos;

namespace BlueBerry24.Services.ProductAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> GetByIdAsync(int id);
        Task<ProductDto> GetByNameAsync(string name);
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto> CreateAsync(CreateProductDto productDto);
        Task<ProductDto> UpdateAsync(int id, UpdateProductDto productDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByNameAsync(string name);
    }
}
