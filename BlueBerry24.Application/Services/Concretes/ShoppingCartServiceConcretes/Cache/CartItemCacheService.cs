using AutoMapper;
using BlueBerry24.Application.Config.Settings;
using BlueBerry24.Application.Dtos.ShoppingCartDtos;
using BlueBerry24.Application.Services.Interfaces.AuthServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces.Cache;
using BlueBerry24.Domain.Entities.ShoppingCart;
using BlueBerry24.Domain.Repositories;
using BlueBerry24.Domain.Repositories.ShoppingCartInterfaces.Cache;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace BlueBerry24.Application.Services.Concretes.ShoppingCartServiceConcretes.Cache
{
    public class CartItemCacheService : ICartItemCacheService
    {
        private readonly ICartItemCacheRepository _cartItemCacheRepository;
        private readonly ICartHeaderCacheRepository _cartHeaderCacheRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly CartSettings _cartSettings;
        public CartItemCacheService(ICartItemCacheRepository cartItemCacheRepository, 
                                    IMapper mapper,
                                    IUnitOfWork unitOfWork,
                                    IUserService userService,
                                    IOptions<CartSettings> options,
                                    ICartHeaderCacheRepository cartHeaderCacheRepository)
        {
            _cartItemCacheRepository = cartItemCacheRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userService = userService;
            _cartSettings = options.Value;
            _cartHeaderCacheRepository = cartHeaderCacheRepository;
        }

        public async Task<bool> AddItemAsync(CartItemDto item, int userId)
        {
            if (!await _userService.IsUserExistsByIdAsync(userId))
            {
                return false;
            }

            var itemsKey = $"{_cartSettings.CartItems}:{userId}";
            var headerKey = $"{_cartSettings.CartHeader}:{userId}";

            if (await _cartHeaderCacheRepository.ExistsByUserIdAsync(headerKey))
            {
                // add item normally to cart.
            }

            var mappedItem = _mapper.Map<CartItem>(item);

            _unitOfWork.BeginCacheTransaction();

            await _unitOfWork.ExecuteInTransactionAsync(async x =>
            {
                var createdHeader = await _cartHeaderCacheRepository.CreateCartHeaderAsync(userId, key, TimeSpan.FromHours(24));
                var addedItem = await _cartItemCacheRepository.AddItemAsync(mappedItem, itemsKey, x);
            });

            var commited = await _unitOfWork.CacheCommitTransactionAsync();

            return commited;
            
        }

        public Task<bool> DecreaseItemAsync(CartItem item, int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAllItems(int userId)
        {
            if (!await _userService.IsUserExistsByIdAsync(userId))
            {
                return false;
            }

            var itemsKey = $"{_cartSettings.CartItems}:{userId}";
            var deletedItems = await _cartItemCacheRepository.DeleteAllItems(itemsKey);

            return deletedItems;
        }

        public async Task<List<CartItemDto>> GetAllItems(int userId)
        {
            if (!await _userService.IsUserExistsByIdAsync(userId))
            {
                return null;
            }

            var itemsKey = $"{_cartSettings.CartItems}:{userId}";

            var items = await _cartItemCacheRepository.GetAllItems(itemsKey);

            var mappedItems = _mapper.Map<List<CartItemDto>>(items);

            return mappedItems;
        }

        public async Task<bool> IncreaseItemAsync(CartItem item, int userId)
        {
            if (!await _userService.IsUserExistsByIdAsync(userId))
            {
                return false;
            }

            var itemsKey = $"{_cartSettings.CartItems}:{userId}";

            var increasedItem = await _cartItemCacheRepository.IncreaseItemAsync(item, itemsKey);

            return increasedItem;
        }

        public async Task<bool> RemoveItemAsync(CartItem item, int userId)
        {
            if (!await _userService.IsUserExistsByIdAsync(userId))
            {
                return false;
            }

            var itemsKey = $"{_cartSettings.CartItems}:{userId}";

            var deletedItem = await _cartItemCacheRepository.RemoveItemAsync(item, itemsKey);
            return deletedItem;
        }

        public async Task<bool> UpdateItemCountAsync(CartItem item, int userId, int newCount)
        {
            if (!await _userService.IsUserExistsByIdAsync(userId))
            {
                return false;
            }

            if(newCount <= 0)
            {
                return false;
            }

            var itemsKey = $"{_cartSettings.CartItems}:{userId}";

            var updatedItem = await _cartItemCacheRepository.UpdateItemCountAsync(item, itemsKey, newCount);
            return updatedItem;
        }
    }
}
