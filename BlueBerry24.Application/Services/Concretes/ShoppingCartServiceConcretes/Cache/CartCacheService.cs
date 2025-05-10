using BlueBerry24.Application.Dtos.ShoppingCartDtos;
using BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Services.Concretes.ShoppingCartServiceConcretes.Cache
{
    public class CartCacheService : ICartCacheService
    {
        private readonly ICartHeaderCacheService _cartHeaderCacheService;
        private readonly ICartItemCacheService _cartItemCacheService;

        public CartCacheService(ICartHeaderCacheService cartHeaderCacheService,
                                ICartItemCacheService cartItemCacheService)
        {
            _cartHeaderCacheService = cartHeaderCacheService;
            _cartItemCacheService = cartItemCacheService;
        }
        public async Task<bool> DeleteCartAsync(int userId)
        {
            return await _cartHeaderCacheService.DeleteCartHeaderAsync(userId);
        }

        public Task<IEnumerable<string>> GetAllActiveCartUserIdsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<CartDto> GetCartAsync(int userId)
        {
            var cartHeader = await _cartHeaderCacheService.GetCartHeaderAsync(userId);
            var items = await _cartItemCacheService.GetAllItems(userId);

            if(cartHeader == null || items == null)
            {
                return null;
            }

            CartDto cart = new CartDto
            {
                CartItems = items,
                ShoppingCartHeaderDto = cartHeader
            };

            return cart;
        }

        //public Task SetCartAsync(string userId, CartDto cart, TimeSpan timeSpan)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
