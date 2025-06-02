using BlueBerry24.Domain.Entities.WishlistEntities;
using BlueBerry24.Domain.Repositories.WishlistInterfaces;
using BlueBerry24.Infrastructure.Data;
using BlueBerry24.Application.Dtos.WishlistDtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Infrastructure.Repositories.WishlistConcretes
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly ApplicationDbContext _context;

        public WishlistRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Wishlist> GetByIdAsync(int id)
        {
            return await _context.Wishlists
                .Include(w => w.WishlistItems)
                    .ThenInclude(wi => wi.Product)
                        .ThenInclude(p => p.ProductCategories)
                            .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<Wishlist> GetUserDefaultWishlistAsync(int userId)
        {
            var defaultWishlist = await _context.Wishlists
                .Include(w => w.WishlistItems)
                    .ThenInclude(wi => wi.Product)
                        .ThenInclude(p => p.ProductCategories)
                            .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(w => w.UserId == userId && w.IsDefault);

            if (defaultWishlist == null)
            {
                defaultWishlist = new Wishlist
                {
                    UserId = userId,
                    Name = "My Wishlist",
                    IsDefault = true,
                    IsPublic = false
                };

                _context.Wishlists.Add(defaultWishlist);
                await _context.SaveChangesAsync();

                return await GetByIdAsync(defaultWishlist.Id);
            }

            return defaultWishlist;
        }

        public async Task<IEnumerable<Wishlist>> GetUserWishlistsAsync(int userId)
        {
            return await _context.Wishlists
                .Include(w => w.WishlistItems)
                    .ThenInclude(wi => wi.Product)
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.IsDefault)
                .ThenByDescending(w => w.UpdatedAt)
                .ToListAsync();
        }

        public async Task<Wishlist> CreateAsync(Wishlist wishlist)
        {
            _context.Wishlists.Add(wishlist);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(wishlist.Id);
        }

        public async Task<Wishlist> UpdateAsync(Wishlist wishlist)
        {
            _context.Wishlists.Update(wishlist);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(wishlist.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var wishlist = await _context.Wishlists.FindAsync(id);
            if (wishlist == null) return false;

            _context.Wishlists.Remove(wishlist);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Wishlists.AnyAsync(w => w.Id == id);
        }

        public async Task<WishlistItem> GetWishlistItemAsync(int wishlistId, int productId)
        {
            return await _context.WishlistItems
                .Include(wi => wi.Product)
                .FirstOrDefaultAsync(wi => wi.WishlistId == wishlistId && wi.ProductId == productId);
        }

        public async Task<WishlistItem> AddItemAsync(WishlistItem item)
        {
            _context.WishlistItems.Add(item);
            await _context.SaveChangesAsync();
            return await _context.WishlistItems
                .Include(wi => wi.Product)
                .FirstOrDefaultAsync(wi => wi.Id == item.Id);
        }

        public async Task<WishlistItem> UpdateItemAsync(WishlistItem item)
        {
            _context.WishlistItems.Update(item);
            await _context.SaveChangesAsync();
            return await _context.WishlistItems
                .Include(wi => wi.Product)
                .FirstOrDefaultAsync(wi => wi.Id == item.Id);
        }

        public async Task<bool> RemoveItemAsync(int wishlistId, int productId)
        {
            var item = await _context.WishlistItems
                .FirstOrDefaultAsync(wi => wi.WishlistId == wishlistId && wi.ProductId == productId);

            if (item == null) return false;

            _context.WishlistItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsProductInWishlistAsync(int userId, int productId)
        {
            return await _context.WishlistItems
                .AnyAsync(wi => wi.Wishlist.UserId == userId && wi.ProductId == productId);
        }

        public async Task<IEnumerable<WishlistItem>> GetWishlistItemsAsync(int wishlistId)
        {
            return await _context.WishlistItems
                .Include(wi => wi.Product)
                    .ThenInclude(p => p.ProductCategories)
                        .ThenInclude(pc => pc.Category)
                .Where(wi => wi.WishlistId == wishlistId)
                .OrderByDescending(wi => wi.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetUserWishlistCountAsync(int userId)
        {
            return await _context.Wishlists.CountAsync(w => w.UserId == userId);
        }

        public async Task<int> GetUserTotalItemsAsync(int userId)
        {
            return await _context.WishlistItems
                .CountAsync(wi => wi.Wishlist.UserId == userId);
        }

        public async Task<decimal> GetUserTotalValueAsync(int userId)
        {
            return await _context.WishlistItems
                .Where(wi => wi.Wishlist.UserId == userId)
                .SumAsync(wi => wi.Product.Price);
        }

        public async Task<IEnumerable<Wishlist>> GetAllWishlistsAsync()
        {
            return await _context.Wishlists
                .Include(w => w.WishlistItems)
                    .ThenInclude(wi => wi.Product)
                        .ThenInclude(p => p.ProductCategories)
                            .ThenInclude(pc => pc.Category)
                .Include(w => w.User)
                .OrderByDescending(w => w.UpdatedAt)
                .ToListAsync();
        }

        public async Task<GlobalWishlistStats> GetGlobalStatsAsync()
        {
            var totalUsers = await _context.Users.CountAsync();
            var totalWishlists = await _context.Wishlists.CountAsync();
            var totalItems = await _context.WishlistItems.CountAsync();
            var totalValue = await _context.WishlistItems.SumAsync(wi => wi.Product.Price);
            var publicWishlists = await _context.Wishlists.CountAsync(w => w.IsPublic);
            var privateWishlists = totalWishlists - publicWishlists;

            var averageItemsPerWishlist = totalWishlists > 0 ? (double)totalItems / totalWishlists : 0;
            var averageWishlistsPerUser = totalUsers > 0 ? (double)totalWishlists / totalUsers : 0;

            var recentActivity = new List<RecentActivity>();
            var today = DateTime.Today;

            for (int i = 0; i < 5; i++)
            {
                var date = today.AddDays(-i);
                var newWishlists = await _context.Wishlists
                    .CountAsync(w => w.CreatedAt.Date == date);
                var newItems = await _context.WishlistItems
                    .CountAsync(wi => wi.CreatedAt.Date == date);

                recentActivity.Add(new RecentActivity
                {
                    Date = date.ToString("yyyy-MM-dd"),
                    NewWishlists = newWishlists,
                    NewItems = newItems
                });
            }

            return new GlobalWishlistStats
            {
                TotalUsers = totalUsers,
                TotalWishlists = totalWishlists,
                TotalItems = totalItems,
                TotalValue = totalValue,
                AverageItemsPerWishlist = averageItemsPerWishlist,
                AverageWishlistsPerUser = averageWishlistsPerUser,
                PublicWishlists = publicWishlists,
                PrivateWishlists = privateWishlists,
                RecentActivity = recentActivity
            };
        }
    }
}
