using AutoMapper;
using BlueBerry24.Services.ShoppingCartAPI.Data;
using BlueBerry24.Services.ShoppingCartAPI.Exceptions;
using BlueBerry24.Services.ShoppingCartAPI.Messaging.Client;
using BlueBerry24.Services.ShoppingCartAPI.Models;
using BlueBerry24.Services.ShoppingCartAPI.Models.DTOs;
using BlueBerry24.Services.ShoppingCartAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Index.HPRtree;
using Newtonsoft.Json;

namespace BlueBerry24.Services.ShoppingCartAPI.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;
        private readonly ILogger<CartService> _logger;
        private readonly ICartCacheService _cacheService;
        private readonly IDistributedLockService _distributedLockService;

        public CartService(ApplicationDbContext context,
                           IMapper mapper,
                           IHttpClientFactory httpClientFactory,
                           IConfiguration config,
                           ILogger<CartService> logger,
                           IDistributedLockService distributedLockService,
                           ICartCacheService cacheService)
        {
            _context = context;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _config = config;
            _logger = logger;
            _distributedLockService = distributedLockService;
            _cacheService = cacheService;
        }




        public async Task<bool> AddItemAsync(string userId, string headerId, CartItemDto itemDto)
        {
            var lockKey = $"lock:cart:{userId}";
            var lockValue = Guid.NewGuid().ToString();

            if (!await _distributedLockService.AcquireLockAsync(lockKey, lockValue, TimeSpan.FromSeconds(10)))
            {
                throw new Exception("Another process is modifying the cart, please again");
            }

            try
            {
                var cart = await _cacheService.GetCartAsync(userId);

                if (cart == null)
                {
                    cart = await LoadCartFromDatabaseAsync(userId, headerId) ?? await CreateNewCartAsync(userId);
                }

                var existingItem = cart.CartItems.FirstOrDefault(i => i.ProductId == itemDto.ProductId);

                if (existingItem != null)
                {
                    existingItem.Count++;
                }
                else
                {
                    itemDto.Count = 1;
                    cart.CartItems.Add(itemDto);
                }

                await _cacheService.SetCartAsync(userId, cart, TimeSpan.FromMinutes(60));
                await SyncCartToDatabaseAsync(cart);

                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding item to cart.");
                throw;
            }

            finally
            {

                await _distributedLockService.ReleaseLockAsync(lockKey, lockValue);
            }
        }




        private async Task<CartDto> LoadCartFromDatabaseAsync(string userId, string headerId)
        {
            var header = await _context.CartHeaders.Where(i => i.UserId == userId && i.Id == headerId).FirstOrDefaultAsync();

            if (header == null) return null;

            return new CartDto
            {
                ShoppingCartHeaderDto = new CartHeaderDto
                {
                    Id = header.Id,
                    CartTotal = header.CartTotal,
                    CouponCode = header.CouponCode,
                    Discount = header.Discount,
                    IsActive = header.IsActive,
                    UserId = header.UserId
                },
                CartItems = header.CartItems.Select(i => new CartItemDto
                {
                    Id = i.Id,
                    CartHeaderId = i.CartHeaderId,
                    ProductId = i.ProductId,
                    ShopId = i.ShopId,
                    Count = i.Count
                }).ToList()
            };
        }


        private async Task<CartDto> CreateNewCartAsync(string userId)
        {
            var newHeader = new CartHeader
            {
                UserId = userId,
                CartTotal = 0m,
                Discount = 0m,
                CouponCode = string.Empty,
                IsActive = true
            };

            await _context.CartHeaders.AddAsync(newHeader);
            await _context.SaveChangesAsync();

            return new CartDto
            {
                ShoppingCartHeaderDto = new CartHeaderDto
                {
                    Id = newHeader.Id,
                    UserId = newHeader.UserId,
                    CartTotal = newHeader.CartTotal,
                    Discount = newHeader.Discount,
                    CouponCode = newHeader.CouponCode,
                    IsActive = newHeader.IsActive
                },

                CartItems = new List<CartItemDto>()
            };


        }

        private async Task SyncCartToDatabaseAsync(CartDto Cart)
        {
            var header = await _context.CartHeaders.FirstOrDefaultAsync(i => i.Id == Cart.ShoppingCartHeaderDto.Id);

            header.CartTotal = Cart.ShoppingCartHeaderDto.CartTotal;
            header.Discount = Cart.ShoppingCartHeaderDto.Discount;
            header.CouponCode = Cart.ShoppingCartHeaderDto.CouponCode;
            header.IsActive = Cart.ShoppingCartHeaderDto.IsActive;

            foreach(var item in Cart.CartItems)
            {
                var dbItem = await _context.CartItems.FirstOrDefaultAsync(i => i.CartHeaderId == header.Id && i.ProductId == item.ProductId);

                if(dbItem != null)
                {
                    dbItem.Count = item.Count;
                }
                else
                {
                    var newItem = new CartItem
                    {
                        CartHeaderId = header.Id,
                        ProductId = item.ProductId,
                        Count = item.Count,
                        ShopId = item.ShopId
                    };
                    await _context.CartItems.AddAsync(newItem);
                }
            }

            await _context.SaveChangesAsync();
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
            if (userId == null || headerId == null)
            {
                throw new Exception("userId or headerId is not");
            }

            var existingHeader = await _context.CartHeaders.FirstOrDefaultAsync(i => i.Id == headerId);

            if (existingHeader != null)
            {
                var cartItems = await _context.CartItems.Where(h => h.CartHeaderId == headerId).ToListAsync();

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

            if (cartHeader == null)
            {
                throw new NotFoundException("The shopping cart not found");
            }

            var item = await _context.CartItems.FirstOrDefaultAsync(h => h.CartHeaderId == headerId && h.ProductId == productId);

            if (item == null)
            {
                throw new NotFoundException("The item was not found.");
            }


            if (item.Count == 1)
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

            if (cartHeader == null)
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

            if (cartHeader == null)
            {
                throw new Exception("The header not found");
            }

            var item = await _context.CartItems.FirstOrDefaultAsync(i => i.Id == itemId && i.CartHeaderId == headerId);

            if (item == null)
            {
                throw new Exception("The item not found");
            }

            if (newCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(newCount), "Quantity cannot be negative");
            }
            else if (newCount == 0)
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