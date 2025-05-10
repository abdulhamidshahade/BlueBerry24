using BlueBerry24.Domain.Entities.ProductEntities;
using BlueBerry24.Domain.Repositories.ProductInterfaces;
using BlueBerry24.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry24.Infrastructure.Repositories.ProductConcretes
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<bool> DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.Categories.AnyAsync(i => i.Id == id);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Categories.AnyAsync(n => n.Name == name);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories;
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            var category = await _context.Categories.Where(i => i.Id == id).FirstOrDefaultAsync();
            return category;
        }

        public async Task<Category> GetByNameAsync(string name)
        {
            var category = await _context.Categories.Where(n => n.Name == name).FirstOrDefaultAsync();
            return category;
        }

        public async Task<Category> UpdateAsync(int id, Category category)
        {
            var categoryModel = await _context.Categories.FindAsync(id);

            categoryModel.Description = category.Description;
            categoryModel.Name = category.Name;
            categoryModel.ImageUrl = category.ImageUrl;

            _context.Categories.Update(categoryModel);
            await _context.SaveChangesAsync();

            return categoryModel;
        }
    }
}
