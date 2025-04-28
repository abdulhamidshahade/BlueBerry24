using BlueBerry24.Domain.Entities.ShoppingCart;
using BlueBerry24.Domain.Repositories.ShoppingCartInterfaces;
using BlueBerry24.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Infrastructure.Repositories.ShoppingCartConcretes
{
    class CartHeaderRepository : ICartHeaderRepository
    {
        private readonly ApplicationDbContext _context;

        public CartHeaderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<CartHeader> CreateCartAsync(CartHeader header)
        {
            await _context.CartHeaders.AddAsync(header);
            await _context.SaveChangesAsync();

            return header;
        }

        public async Task<bool> DeleteCartHeaderAsync(int id)
        {
            var cartHeader = await _context.CartHeaders.FindAsync(id);

            _context.CartHeaders.Remove(cartHeader);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.CartHeaders.AnyAsync(i => i.Id == id);
        }

        public async Task<CartHeader> UpdateCartHeaderAsync(int id, CartHeader header)
        {
            var headerModel = await _context.CartHeaders.FindAsync(id);

            headerModel.CartTotal = header.CartTotal;
            headerModel.Discount = header.Discount;
            headerModel.IsActive = header.IsActive;
            headerModel.CouponCode = header.CouponCode;

            await _context.SaveChangesAsync();
            return headerModel;
        }
    }
}
