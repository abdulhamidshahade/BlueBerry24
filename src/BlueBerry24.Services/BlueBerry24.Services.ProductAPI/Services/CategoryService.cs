using AutoMapper;
using BlueBerry24.Services.ProductAPI.Exceptions;
using BlueBerry24.Services.ProductAPI.Models;
using BlueBerry24.Services.ProductAPI.Models.DTOs.CategoryDtos;
using BlueBerry24.Services.ProductAPI.Services.Generic;
using BlueBerry24.Services.ProductAPI.Services.Interfaces;

namespace BlueBerry24.Services.ProductAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IRepository<Category> categoryRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                throw new NotFoundException($"Category with id {id} not found");
            }

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name cannot be empty", nameof(name));
            }

            var category = await _categoryRepository.GetAsync(c => c.Name == name);
            
            if (category == null)
            {
                throw new NotFoundException($"Category with name: {name} not found");
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
                throw new ArgumentNullException(nameof(categoryDto));
            }

            if (await ExistsByNameAsync(categoryDto.Name))
            {
                throw new DuplicateEntityException($"Category with name: {categoryDto.Name} already exists");
            }

            var category = _mapper.Map<Category>(categoryDto);

            await _categoryRepository.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(category);
        }


        public async Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                throw new ArgumentNullException(nameof(categoryDto));
            }

            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null)
            {
                throw new NotFoundException($"Category with id: {id} not found");
            }

            var categoryWithName = await _categoryRepository.GetAsync(c => c.Name == categoryDto.Name && c.Id != id);

            if (categoryWithName != null)
            {
                throw new DuplicateEntityException($"Category with Name: {categoryDto.Name} already exists");
            }

            existingCategory.Name = categoryDto.Name;
            existingCategory.Description = categoryDto.Description;
            existingCategory.ImageUrl = categoryDto.ImageUrl;

            _categoryRepository.Update(existingCategory);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(existingCategory);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return false;
            }

            _categoryRepository.Delete(category);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _categoryRepository.ExistsAsync(i => i.Id == id);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"Category name: {name} cannot be empty", nameof(name));
            }

            return await _categoryRepository.ExistsAsync(c => c.Name == name);
        }
    }
}
