using BlueBerry24.Application.Dtos;
using BlueBerry24.Application.Dtos.ProductDtos;

namespace BlueBerry24.Application.Services.Interfaces.ProductServiceInterfaces
{
    public interface IProductService
    {
        Task<IReadOnlyList<ProductDto>> GetAllAsync();
        Task<PaginationDto<ProductDto>> GetPaginatedAsync(ProductFilterDto filter);
        Task<ProductDto> GetByIdAsync(int id);
        Task<ProductDto> GetByNameAsync(string name);
        Task<ProductDto> CreateAsync(CreateProductDto productDto, List<int> categories);
        Task<ProductDto> UpdateAsync(int id, UpdateProductDto productDto, List<int> categories);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
        Task<bool> ExistsByNameAsync(string name);
    }
}
