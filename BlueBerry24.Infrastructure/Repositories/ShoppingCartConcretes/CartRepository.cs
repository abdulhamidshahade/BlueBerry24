using BlueBerry24.Domain.Constants;
using BlueBerry24.Domain.Entities.ShoppingCartEntities;
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
        public async Task<Cart> CreateCartAsync(int? userId, CartStatus status)
        {
            var shoppingCart = new Cart
            {
                UserId = userId,
                Status = status
            };


            await _context.ShoppingCarts.AddAsync(shoppingCart);
            await _unitOfWork.SaveDbChangesAsync();

            return shoppingCart;
        }

        public async Task<Cart> CreateCartAsync(string? sessionId, CartStatus status)
        {
            var shoppingCart = new Cart
            {
                SessionId = sessionId,
                Status = status
            };


            await _context.ShoppingCarts.AddAsync(shoppingCart);
            await _unitOfWork.SaveDbChangesAsync();

            return shoppingCart;
        }

        public async Task<bool> DeleteCartAsync(int? userId, string? sessionId)
        {
            Cart? shoppingCart = null;

            if (userId.HasValue)
            {
                shoppingCart = await _context.ShoppingCarts
                    .Where(i => i.UserId == userId && i.Status == CartStatus.Active)
                    .FirstOrDefaultAsync();
            }
            else
            {
                shoppingCart = await _context.ShoppingCarts
                    .Where(s => s.SessionId == sessionId && s.Status == CartStatus.Active)
                    .FirstOrDefaultAsync();
            }

            _context.ShoppingCarts.Remove(shoppingCart);
            return await _unitOfWork.SaveDbChangesAsync();
        }

        public async Task<Cart> GetCartByUserIdAsync(int? userId, CartStatus? status = CartStatus.Active)
        {
            var shoppingCart = await _context.ShoppingCarts
                .Where(i => i.UserId == userId && i.Status == status)
                .Include(c => c.CartItems)
                .ThenInclude(p => p.Product)
                .Include(c => c.CartCoupons)
                .ThenInclude(c => c.Coupon)
                .FirstOrDefaultAsync();

            return shoppingCart;
        }

        public async Task<Cart> GetCartBySessionIdAsync(string sessionId, CartStatus? status = CartStatus.Active)
        {
            var shoppingCart = await _context.ShoppingCarts
                .Where(i => i.SessionId == sessionId && i.Status == status)
                .Include(c => c.CartItems)
                .ThenInclude(p => p.Product)
                .Include(c => c.CartCoupons)
                .ThenInclude(c => c.Coupon)
                .FirstOrDefaultAsync();

            return shoppingCart;
        }

        public async Task<List<Cart>> GetCartsAsync()
        {
            var shoppingCarts = await _context.ShoppingCarts.ToListAsync();
            return shoppingCarts;
        }

        public async Task<Cart> UpdateCartStatusAsync(int? userId, CartStatus status = CartStatus.Converted)
        {
            Cart? shoppingCart = null;

            if (userId.HasValue)
            {
                shoppingCart = await _context.ShoppingCarts
                    .Where(i => i.UserId == userId).FirstOrDefaultAsync();
            }
            else
            {
                shoppingCart = await _context.ShoppingCarts
                    .Where(i => i.UserId == userId).FirstOrDefaultAsync();
            }

            shoppingCart.Status = status;

            await _unitOfWork.SaveDbChangesAsync();

            return shoppingCart;
        }


        public async Task<Cart> UpdateItemQuantityAsync(int? userId, string? sessionId, int productId, int quantity)
        {
            Cart? shoppingCart = null;

            if (userId.HasValue)
            {
                shoppingCart = await _context.ShoppingCarts
                    .Where(c => c.UserId == userId && c.Status == CartStatus.Active)
                    .Include(c => c.CartItems)
                    .ThenInclude(p => p.Product)
                    .Include(c => c.CartCoupons)
                    .ThenInclude(c => c.Coupon)
                    .FirstOrDefaultAsync();
            }
            else if (!string.IsNullOrEmpty(sessionId))
            {
                shoppingCart = await _context.ShoppingCarts
                    .Where(c => c.SessionId == sessionId && c.Status == CartStatus.Active)
                    .Include(c => c.CartItems)
                    .ThenInclude(p => p.Product)
                    .Include(c => c.CartCoupons)
                    .ThenInclude(c => c.Coupon)
                    .FirstOrDefaultAsync();
            }

            if (shoppingCart == null)
            {
                return null;
            }

            var itemToUpdate = shoppingCart.CartItems.Where(i => i.ProductId == productId).FirstOrDefault();
            if (itemToUpdate == null)
            {
                return null;
            }

            itemToUpdate.Quantity = quantity;

            if (await _unitOfWork.SaveDbChangesAsync())
            {
                return shoppingCart;
            }

            return null;
        }


        public async Task<CartItem> CreateItemAsync(int cartId, int? userId, string? sessionId, int productId, int quantity,
            decimal unitPrice)
        {
            CartItem item = null;

            if (userId.HasValue)
            {
                item = new CartItem
                {
                    ProductId = productId,
                    UserId = userId,
                    Quantity = quantity,
                    ShoppingCartId = cartId,
                    UnitPrice = unitPrice
                };

            }
            else
            {
                item = new CartItem
                {
                    ProductId = productId,
                    SessionId = sessionId,
                    Quantity = quantity,
                    ShoppingCartId = cartId,
                    UnitPrice = unitPrice
                };
            }

            var createdItem = await _context.CartItems.AddAsync(item);

            if (await _unitOfWork.SaveDbChangesAsync())
            {
                return item;
            }

            return null;
        }

        public async Task<Cart> UpdateCartAsync(Cart cart)
        {
            var updatedCart = _context.ShoppingCarts.Update(cart);

            if (await _unitOfWork.SaveDbChangesAsync())
            {
                return cart;
            }

            return null;
        }



        public async Task<bool> RemoveItemAsync(int? userId, string? sessionId, int productId)
        {
            Cart? cart = null;

            if (userId.HasValue)
            {
                cart = await _context.ShoppingCarts
                    .Where(c => c.UserId == userId && c.Status == CartStatus.Active)
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync();
            }
            else if (!string.IsNullOrEmpty(sessionId))
            {
                cart = await _context.ShoppingCarts
                    .Where(c => c.SessionId == sessionId && c.Status == CartStatus.Active)
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync();
            }

            if (cart == null)
            {
                return false;
            }

            var itemToRemove = cart.CartItems.Where(i => i.ProductId == productId).FirstOrDefault();
            if (itemToRemove == null)
            {
                return false;
            }

            _context.CartItems.Remove(itemToRemove);

            return await _unitOfWork.SaveDbChangesAsync();
        }

        public async Task<Cart> GetCartByIdAsync(int cartId, CartStatus status)
        {
            var shoppingCart = await _context.ShoppingCarts
                .Where(i => i.Id == cartId && i.Status == status)
                .Include(c => c.CartItems)
                .ThenInclude(p => p.Product)
                .Include(c => c.CartCoupons)
                .ThenInclude(c => c.Coupon)
                .FirstOrDefaultAsync();

            return shoppingCart;
        }

        public async Task<bool> UpdateItemsAsync(List<CartItem> items)
        {
            _context.CartItems.UpdateRange(items);

            return await _unitOfWork.SaveDbChangesAsync();
        }

        public async Task<bool> DeleteCartById(int Id)
        {
            Cart? shoppingCart = null;

            if (Id > 0)
            {
                shoppingCart = await _context.ShoppingCarts
                    .Where(i => i.Id == Id && i.Status == CartStatus.Active)
                    .FirstOrDefaultAsync();
            }

            _context.ShoppingCarts.Remove(shoppingCart);
            return await _unitOfWork.SaveDbChangesAsync();
        }
    }
}
