using BlueBerry24.Domain.Entities.Product;
using BlueBerry24.Domain.Repositories.ProductInterfaces;
using BlueBerry24.Infrastructure.Data;

namespace BlueBerry24.Infrastructure.Repositories.ProductConcretes
{
    class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
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
            return await _context.SaveChangesAsync() > 0;
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
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
