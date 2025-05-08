using AutoMapper;
using BlueBerry24.Application.Dtos.ShoppingCartDtos;
using BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces;
using BlueBerry24.Domain.Repositories;
using BlueBerry24.Domain.Repositories.ShoppingCartInterfaces;
using System.Runtime.InteropServices;

namespace BlueBerry24.Application.Services.Concretes.ShoppingCartServiceConcretes
{
    public class CartService : ICartService
    {
        private readonly ICartHeaderService _cartHeaderService;
        private readonly ICartItemService _cartItemService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartRepository _cartRepository;

        public CartService(ICartHeaderService cartHeaderService,
                           ICartItemService cartItemService,
                           IMapper mapper,
                           IUnitOfWork unitOfWork,
                           ICartRepository cartRepository)
        {
            _cartHeaderService = cartHeaderService;
            _cartItemService = cartItemService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cartRepository = cartRepository;
        }


        public async Task<CartDto> GetCartAsync(int userId)
        {
            var cartHeader = await _cartHeaderService.GetCartHeaderAsync(userId);
            var cartItems = await _cartItemService.GetItemsFromPersistenceStorage(userId);

            var cart = new CartDto
            {
                ShoppingCartHeaderDto = cartHeader,
                CartItems = cartItems,
                UserId = userId
            };

            return cart;
        }

        public async Task<bool> DeleteCartAsync(int userId)
        {
            await _unitOfWork.BeginTransactionAsync();

            var deletedCartHeader = await _cartHeaderService.DeleteCartHeaderAsync(userId);
            var deletedItems = await _cartItemService.RemoveItemsFromPersistenceStorage(userId);

            if(!deletedCartHeader || !deletedItems)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return false;
            }
            else
            {
                return await _unitOfWork.CommitTransactionAsync();
            }
        }

        public async Task<CartDto> UpdateCartAsync(int userId, bool isActive)
        {
            var updatedCart = await _cartRepository.UpdateCartAsync(userId, isActive);
            return _mapper.Map<CartDto>(updatedCart);
        }

        public async Task<List<CartDto>> GetCartsAsync()
        {
            var carts = await _cartRepository.GetCartsAsync();

            return _mapper.Map<List<CartDto>>(carts);
        }

        public async Task<bool> IsCartAsync(int userId)
        {
            var isExists = await _cartRepository.GetCartAsync(userId);
            return isExists != null;
        }


        public async Task<CartDto> CreateCartAsync(int userId, int headerId)
        {
            var createdCart = await _cartRepository.CreateCartAsync(userId, headerId);

            return _mapper.Map<CartDto>(createdCart);
        }

    }
}