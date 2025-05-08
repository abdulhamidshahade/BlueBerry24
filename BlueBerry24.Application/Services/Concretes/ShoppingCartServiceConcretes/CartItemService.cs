using AutoMapper;
using BlueBerry24.Application.Dtos.ShoppingCartDtos;
using BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces;
using BlueBerry24.Domain.Entities.ShoppingCart;
using BlueBerry24.Domain.Repositories.ShoppingCartInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Services.Concretes.ShoppingCartServiceConcretes
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IMapper _mapper;
        private readonly ICartHeaderRepository _cartHeaderRepository;

        public CartItemService(ICartItemRepository cartItemRepository, IMapper mapper, ICartHeaderRepository cartHeaderRepository)
        {
            _cartItemRepository = cartItemRepository;
            _mapper = mapper;
            _cartHeaderRepository = cartHeaderRepository;
        }

        public async Task<List<CartItemDto>> GetItemsFromPersistenceStorage(int cartId)
        {
            var cartItems = await _cartItemRepository.GetItemsFromPersistenceStorage(cartId);

            if (cartItems == null) return null;

            return _mapper.Map<List<CartItemDto>>(cartItems);
        }

        public async Task<bool> LoadItemsToPersistenceStorage(List<CartItem> items, int cartId)
        {
            var addedItems = await _cartItemRepository.LoadItemsToPersistenceStorage(items, cartId);

            return addedItems;
        }

        public async Task<bool> RemoveItemsFromPersistenceStorage(int cartId)
        {
            var removedItems = await _cartItemRepository.RemoveItemsFromPersistenceStorage(cartId);

            return removedItems;
        }
    }
}
