using BlueBerry24.Services.ProductAPI.Data;
using BlueBerry24.Services.ProductAPI.Models;
using BlueBerry24.Services.ProductAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry24.Services.ProductAPI.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;
        private readonly IUnitOfWork _unitOfWork;

        public ProductCategoryService(ApplicationDbContext context, 
            IProductService productService,
            IUnitOfWork unitOfWork)
        {
            _context = context;
            _productService = productService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddProductCategoryAsync(Product product, List<int> categories)
        {
            if(categories.Count == 0 || !await _productService.ExistsAsync(product.Id))
            {
                return false;
            }

            foreach(int categoryId in categories)
            {
                await _context.Products_Categories.AddAsync(new ProductCategory
                {
                    CategoryId = categoryId,
                    ProductId = product.Id
                });
            }

            return await _unitOfWork.SaveChangesAsync() > 1;
        }

        public async Task<bool> UpdateProductCategoryAsync(Product product, List<int> categories)
        {
            if (categories.Count == 0 || !await _productService.ExistsAsync(product.Id))
            {
                return false;
            } 

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var existingCategories = await _context.Products_Categories.Where(x => x.ProductId == product.Id).ToListAsync();

                    _context.Products_Categories.RemoveRange(existingCategories);

                    bool result = await AddProductCategoryAsync(product, categories);

                    if (!result)
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }

                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }
    }
}
