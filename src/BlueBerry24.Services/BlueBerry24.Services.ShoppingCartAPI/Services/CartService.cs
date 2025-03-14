using AutoMapper;
using BlueBerry24.Services.ShoppingCartAPI.Data;
using BlueBerry24.Services.ShoppingCartAPI.Exceptions;
using BlueBerry24.Services.ShoppingCartAPI.Models.DTOs;
using BlueBerry24.Services.ShoppingCartAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry24.Services.ShoppingCartAPI.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CartService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Task<CartDto> AddItemAsync(string userId, string headerId, CartItemDto itemDto)
        {
            throw new NotImplementedException();
        }

        public Task<CartDto> ApplyCouponAsync(string userId, string headerId, string couponCode)
        {
            throw new NotImplementedException();
        }

        public Task<CartDto> CreateCartAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<CartDto> DeleteShoppingCartAsync(string userId, string headerId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsByHeaderIdAsync(string userId, string headerId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
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

        public async Task<CartDto> GetShoppingCartByUserIdAsync(string userId)
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

        public Task<bool> RemoveItemAsync(string userId, string headerId, string itemId)
        {
            throw new NotImplementedException();
        }

        public Task<CartDto> UpdateCartHeaderAsync(string userId, string headerId, CartHeaderDto headerDto)
        {
            throw new NotImplementedException();
        }

        public Task<CartDto> UpdateItemCountAsync(string userId, string headerId, string itemId, int newCount)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateCouponAsync(string userId, string couponCode)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateShopOwner(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
