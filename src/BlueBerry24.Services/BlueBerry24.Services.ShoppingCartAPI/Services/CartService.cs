using AutoMapper;
using BlueBerry24.Services.ShoppingCartAPI.Data;
using BlueBerry24.Services.ShoppingCartAPI.Exceptions;
using BlueBerry24.Services.ShoppingCartAPI.Messaging.Client;
using BlueBerry24.Services.ShoppingCartAPI.Models;
using BlueBerry24.Services.ShoppingCartAPI.Models.DTOs;
using BlueBerry24.Services.ShoppingCartAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BlueBerry24.Services.ShoppingCartAPI.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public CartService(ApplicationDbContext context, IMapper mapper, IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _config = config;

        }
        public async Task<bool> AddItemAsync(string userId, string headerId, CartItemDto itemDto)
        {
            var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(u => u.Id == headerId && u.UserId == userId);

            if(cartHeader == null)
            {
                cartHeader = await CreateCartAsync(userId);

                headerId = cartHeader.Id;
            }

            var productExists = await _context.CartItems.FirstOrDefaultAsync(i => i.ProductId == itemDto.ProductId && i.CartHeaderId == headerId);

            if (productExists != null)
            {
                productExists.Count++;
            }
            else
            {
                var item = new CartItem
                {
                    CartHeaderId = headerId,
                    Count = 1,
                    ProductId = itemDto.ProductId
                };

                await _context.CartHeaders.AddAsync(cartHeader);
            }

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<CartHeader> CreateCartAsync(string userId)
        {
            var cartHeader = new CartHeader
            {
                UserId = userId,
                IsActive = true
            };

            await _context.CartHeaders.AddAsync(cartHeader);
            await _context.SaveChangesAsync();

            return cartHeader;
        }
        public async Task<bool> DeleteShoppingCartAsync(string userId, string headerId)
        {
            var CartHeader = await _context.CartHeaders.FirstOrDefaultAsync(u => u.UserId == userId && u.Id == headerId && u.IsActive);

            _context.CartHeaders.Remove(CartHeader);

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> ExistsByUserIdAsync(string userId, string headerId)
        {
            return await _context.CartHeaders.AnyAsync(u => u.UserId == userId);
        }
        public async Task<CartDto> GetCartByHeaderIdAsync(string userId, string headerId)
        {
            if(userId == null || headerId == null)
            {
                throw new Exception("userId or headerId is not");
            }

            var existingHeader = await _context.CartHeaders.FirstOrDefaultAsync(i => i.Id == headerId);

            if(existingHeader != null)
            {
                var cartItems = await _context.CartItems.Where(h => h.CartHeaderId == headerId).ToListAsync();

                if(cartItems.Count != 0)
                {
                    return new CartDto
                    {
                        CartItems = _mapper.Map<List<CartItemDto>>(cartItems),
                        ShoppingCartHeaderDto = _mapper.Map<CartHeaderDto>(existingHeader)
                    };
                }
                else
                {
                    throw new NotFoundException("There is no items found");
                }
            }
            else
            {
                throw new NotFoundException("There is no cart header found");
            }
        }
        public async Task<CartDto> GetCartByUserIdAsync(string userId)
        {
            if (userId == null)
            {
                throw new Exception("user id is null");
            }

            var existingHeader = await _context.CartHeaders.FirstOrDefaultAsync(i => i.UserId == userId);

            if (existingHeader != null)
            {
                
                var cartItems = await _context.CartItems.Where(h => h.CartHeaderId == existingHeader.Id).ToListAsync();

                if (cartItems.Count != 0)
                {
                    return new CartDto
                    {
                        CartItems = _mapper.Map<List<CartItemDto>>(cartItems),
                        ShoppingCartHeaderDto = _mapper.Map<CartHeaderDto>(existingHeader)
                    };
                }
                else
                {
                    throw new NotFoundException("There is no items found");
                }
            }
            else
            {
                throw new NotFoundException("There is no cart header found");
            }
        }
        public async Task<bool> RemoveItemAsync(string userId, string headerId, string productId)
        {
            var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(u => u.UserId == userId && u.Id == headerId && u.IsActive);

            if(cartHeader == null)
            {
                throw new NotFoundException("The shopping cart not found");
            }

            var item = await _context.CartItems.FirstOrDefaultAsync(h => h.CartHeaderId == headerId && h.ProductId == productId);

            if(item == null)
            {
                throw new NotFoundException("The item was not found.");
            }


            if(item.Count == 1)
            {
                _context.CartItems.Remove(item);
            }
            else
            {
                item.Count--;
            }

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> UpdateCartHeaderAsync(string userId, string headerId, CartHeaderDto headerDto)
        {

            var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(h => h.Id == headerId
                                                                            && h.UserId == userId && h.IsActive);

            if(cartHeader == null)
            {
                throw new NotFoundException("The cart header not found!");
            }

            cartHeader.CartTotal = headerDto.CartTotal;
            cartHeader.Discount = headerDto.Discount;
            cartHeader.CouponCode = headerDto.CouponCode;

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> UpdateItemCountAsync(string userId, string headerId, string itemId, int newCount)
        {
            var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(i =>
                i.Id == headerId &&
                i.UserId == userId &&
                i.IsActive
            );

            if(cartHeader == null)
            {
                throw new Exception("The header not found");
            }

            var item = await _context.CartItems.FirstOrDefaultAsync(i => i.Id == itemId && i.CartHeaderId == headerId);

            if(item == null)
            {
                throw new Exception("The item not found");
            }

            if(newCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(newCount), "Quantity cannot be negative");
            }
            else if(newCount == 0)
            {
                _context.CartItems.Remove(item);
            }
            else
            {
                item.Count = newCount;
            }

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> ExistsByHeaderIdAsync(string userId, string headerId)
        {
            return await _context.CartHeaders.AnyAsync(i => i.Id == headerId && i.UserId == userId);
        }



        
    }
}