using BlueBerry24.Domain.Entities.Shop;
using BlueBerry24.Domain.Repositories.ShopInterfaces;
using BlueBerry24.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace BlueBerry24.Infrastructure.Repositories.ShopConcretes
{
    class ShopRepository : IShopRepository
    {
        private readonly ApplicationDbContext _context;
        
        public ShopRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Shop> CreateAsync(Shop shop)
        {
            await _context.Shops.AddAsync(shop);
            await _context.SaveChangesAsync();

            return shop;
        }

        public async Task<bool> DeleteAsync(Shop shop)
        {
            _context.Shops.Remove(shop);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.Shops.AnyAsync(i => i.Id == id);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Shops.AnyAsync(n => n.Name == name);
        }

        public async Task<IEnumerable<Shop>> GetAllAsync()
        {
            var shops = await _context.Shops.ToListAsync();
            return shops;
        }

        public async Task<Shop> GetByIdAsync(int id)
        {
            var shop = await _context.Shops.FindAsync(id);
            return shop;
        }

        public async Task<Shop> GetByNameAsync(string name)
        {
            var shop = await _context.Shops.Where(n => n.Name == name).FirstOrDefaultAsync();
            return shop;
        }

        public async Task<Shop> UpdateAsync(int id, Shop shop)
        {
            var shopModel = await _context.Shops.FindAsync(id);

            shopModel.Description = shop.Description;
            shopModel.City = shop.City;
            shopModel.Country = shop.Country;
            shopModel.LogoUrl = shop.LogoUrl;
            shopModel.Address = shop.Address;
            shopModel.IsActive = shop.IsActive;
            shopModel.Email = shop.Email;
            shopModel.Phone = shop.Phone;

            await _context.SaveChangesAsync();

            return shopModel;
        }
    }
}
