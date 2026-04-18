using Berryfy.Application.Dtos.CategoryDtos;

namespace Berryfy.Application.Services.Interfaces.ProductServiceInterfaces
{
    public interface ICategoryService
    {
        Task<CategoryDto> GetByIdAsync(int id);
        Task<CategoryDto> GetByNameAsync(string name);
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> CreateAsync(CreateCategoryDto categoryDto);
        Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto categoryDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByNameAsync(string name);
    }
}
