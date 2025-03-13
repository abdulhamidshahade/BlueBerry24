using BlueBerry24.Services.ProductAPI.Models;
using BlueBerry24.Services.ProductAPI.Models.DTOs.CategoryDtos;

namespace BlueBerry24.Services.ProductAPI.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDto> GetByIdAsync(string id);
        Task<CategoryDto> GetByNameAsync(string name);
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> CreateAsync(CreateCategoryDto categoryDto);
        Task<CategoryDto> UpdateAsync(string id, UpdateCategoryDto categoryDto);
        Task<bool> DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<bool> ExistsByNameAsync(string name);
    }
}
