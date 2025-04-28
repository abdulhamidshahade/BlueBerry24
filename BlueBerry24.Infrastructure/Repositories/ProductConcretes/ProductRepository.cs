using BlueBerry24.Domain.Entities.Product;
using BlueBerry24.Domain.Repositories.ProductInterfaces;
using BlueBerry24.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry24.Infrastructure.Repositories.ProductConcretes
{
    class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<bool> DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.Products.AnyAsync(i => i.Id == id);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Products.AnyAsync(n => n.Name == name);
        }


        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> GetByNameAsync(string name)
        {
            var product = await _context.Products.Where(n => n.Name == name).FirstOrDefaultAsync();
            return product;
        }

        public async Task<Product> UpdateAsync(int id, Product product)
        {
            var productModel = await _context.Products.FindAsync(id);

            productModel.Description = product.Description;
            productModel.Price = product.Price;
            productModel.Name = product.Name;
            productModel.StockQuantity = product.StockQuantity;
            productModel.ImageUrl = product.ImageUrl;
            productModel.ShopId = product.ShopId;

            _context.Products.Update(productModel);
            await _context.SaveChangesAsync();

            return productModel;
        }
    }
}
