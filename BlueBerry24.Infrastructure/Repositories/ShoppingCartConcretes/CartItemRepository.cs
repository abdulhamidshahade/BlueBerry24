using BlueBerry24.Domain.Entities.ShoppingCart;
using BlueBerry24.Domain.Repositories.ShoppingCartInterfaces;
using BlueBerry24.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Infrastructure.Repositories.ShoppingCartConcretes
{
    class CartItemRepository : ICartItemRepository
    {
        private readonly ApplicationDbContext _context;
        public CartItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CartItem> AddItemAsync(CartItem item)
        {
            await _context.CartItems.AddAsync(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task<bool> DecreaseItemAsync(CartItem item)
        {
            var itemModel = await _context.CartItems.FindAsync(item.Id);

            if (itemModel == null) return false;

            itemModel.Count--;
            return await _context.SaveChangesAsync() > 0;
        }

        public Task<bool> ExistsByUserIdAsync(int userId, int headerId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IncreaseItemAsync(CartItem item)
        {
            var itemModel = await _context.CartItems.FindAsync(item.Id);

            if (itemModel == null) return false;

            itemModel.Count++;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveItemAsync(CartItem item)
        {
            _context.CartItems.Remove(item);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<CartItem> UpdateItemCountAsync(CartItem item, int newCount)
        {
            var itemModel = await _context.CartItems.FindAsync(item);

            itemModel.Count = newCount;
            await _context.SaveChangesAsync();

            return itemModel;
        }
    }
}
