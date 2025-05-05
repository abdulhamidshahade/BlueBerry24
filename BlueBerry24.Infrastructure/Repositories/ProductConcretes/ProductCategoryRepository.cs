using BlueBerry24.Domain.Entities.Product;
using BlueBerry24.Domain.Repositories;
using BlueBerry24.Domain.Repositories.ProductInterfaces;
using BlueBerry24.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry24.Infrastructure.Repositories.ProductConcretes
{
    class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public ProductCategoryRepository(ApplicationDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> AddProductCategoryAsync(Product product, List<int> categories)
        {
            List<ProductCategory> productCategories = new List<ProductCategory>();

            foreach(var category in categories)
            {
                productCategories.Add(new ProductCategory
                {
                    ProductId = product.Id,
                    CategoryId = category
                });
            }

            await _context.ProductCategories.AddRangeAsync(productCategories);
            return await _unitOfWork.SaveDbChangesAsync();
        }

        public async Task<List<Category>> GetCategoriesByProuductId(int productId)
        {
            var categories = await _context.ProductCategories
                .Where(i => i.ProductId == productId)
                .Select(c => c.Category)
                .ToListAsync();

            if (categories == null) return null;

            return categories;
        }

        public async Task<bool> RemoveCategoriesByProductId(int productId)
        {
            var productCategories = await _context.ProductCategories.Where(i => i.ProductId == productId).ToListAsync();
            _context.ProductCategories.RemoveRange(productCategories);

            return await _unitOfWork.SaveDbChangesAsync();
        }

        public async Task<bool> UpdateProductCategoryAsync(Product product, List<int> categories)
        {
            List<ProductCategory> productCategories = new List<ProductCategory>();

            foreach(var category in categories)
            {
                productCategories.Add(new ProductCategory
                {
                    ProductId = product.Id,
                    CategoryId = category
                });
            }

            await _context.ProductCategories.AddRangeAsync(productCategories);
            return await _unitOfWork.SaveDbChangesAsync();
        }
    }
}
