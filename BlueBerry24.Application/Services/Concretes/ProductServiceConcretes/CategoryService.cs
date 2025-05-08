

using AutoMapper;
using BlueBerry24.Application.Dtos.CategoryDtos;
using BlueBerry24.Application.Services.Interfaces.ProductServiceInterfaces;
using BlueBerry24.Domain.Entities.Product;
using BlueBerry24.Domain.Repositories.ProductInterfaces;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace BlueBerry24.Application.Services.Concretes.ProductServiceConcretes
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return null;
            }

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            var category = await _categoryRepository.GetByNameAsync(name);
            
            if (category == null)
            {
                return null;
            }

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                return null;
            }

            if (await ExistsByNameAsync(categoryDto.Name))
            {
                return null;
            }

            var category = _mapper.Map<Category>(categoryDto);

            var createdCategory = await _categoryRepository.CreateAsync(category);
            return _mapper.Map<CategoryDto>(createdCategory);
        }


        public async Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                return null;
            }

            //var existingCategory = await _categoryRepository.GetByIdAsync(id);
            //if (existingCategory == null)
            //{
            //    return null;
            //}

            //var categoryWithName = await _categoryRepository.GetAsync(c => c.Name == categoryDto.Name && c.Id != id);

            //if (categoryWithName != null)
            //{
            //    return null;
            //}

            //existingCategory.Name = categoryDto.Name;
            //existingCategory.Description = categoryDto.Description;
            //existingCategory.ImageUrl = categoryDto.ImageUrl;

            var mappedCategory = _mapper.Map<Category>(categoryDto);

            var updatedCategory = await _categoryRepository.UpdateAsync(id, mappedCategory);


            return _mapper.Map<CategoryDto>(updatedCategory);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return false;
            }

            var deletedCategory = await _categoryRepository.DeleteAsync(category);


            return deletedCategory;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _categoryRepository.ExistsByIdAsync(id);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            return await _categoryRepository.ExistsByNameAsync(name);
        }
    }
}
