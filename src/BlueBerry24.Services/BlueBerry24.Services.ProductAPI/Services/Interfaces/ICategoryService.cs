using BlueBerry24.Services.ProductAPI.Models;
using BlueBerry24.Services.ProductAPI.Models.DTOs.CategoryDtos;

namespace BlueBerry24.Services.ProductAPI.Services.Interfaces
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
