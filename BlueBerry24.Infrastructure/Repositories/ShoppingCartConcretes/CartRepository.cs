using BlueBerry24.Domain.Entities.ShoppingCart;
using BlueBerry24.Domain.Repositories;
using BlueBerry24.Domain.Repositories.ShoppingCartInterfaces;
using BlueBerry24.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry24.Infrastructure.Repositories.ShoppingCartConcretes
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public CartRepository(ApplicationDbContext context,
                              IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public async Task<ShoppingCart> CreateCartAsync(int userId, int headerId)
        {
            var shoppingCart = new ShoppingCart
            {
                UserId = userId,
                CartHeaderId = headerId
            };


            await _context.ShoppingCarts.AddAsync(shoppingCart);
            await _unitOfWork.SaveDbChangesAsync();

            return shoppingCart;
        }

        public async Task<bool> DeleteCartAsync(int userId)
        {
            var shoppingCart = await _context.ShoppingCarts.Where(i => i.UserId == userId && i.IsActive)
                .FirstOrDefaultAsync();

            _context.ShoppingCarts.Remove(shoppingCart);
            return await _unitOfWork.SaveDbChangesAsync();
        }

        public async Task<ShoppingCart> GetCartAsync(int userId)
        {
            var shoppingCart = await _context.ShoppingCarts.Where(i => i.UserId == userId && i.IsActive)
                .FirstOrDefaultAsync();

            return shoppingCart;
        }

        public async Task<List<ShoppingCart>> GetCartsAsync()
        {
            var shoppingCarts = await _context.ShoppingCarts.ToListAsync();

            return shoppingCarts;
        }

        public async Task<ShoppingCart> UpdateCartAsync(int userId, bool isActive)
        {
            var shoppingCart = await _context.ShoppingCarts.Where(i => i.UserId == userId).FirstOrDefaultAsync();

            shoppingCart.IsActive = isActive;

            await _unitOfWork.SaveDbChangesAsync();

            return shoppingCart;
        }
    }
}
