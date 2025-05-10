using AutoMapper;
using BlueBerry24.Application.Config.Settings;
using BlueBerry24.Application.Dtos.ShoppingCartDtos;
using BlueBerry24.Application.Services.Interfaces.AuthServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces.Cache;
using BlueBerry24.Domain.Entities.ShoppingCartEntities;
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

            var mappedItem = _mapper.Map<CartItem>(item);
            var itemsKey = $"{_cartSettings.CartItems}:{userId}";
            var headerKey = $"{_cartSettings.CartHeader}:{userId}";

            if (await _cartHeaderCacheRepository.ExistsByUserIdAsync(headerKey))
            {
                var addItem  = await _cartItemCacheRepository.AddItemAsync(mappedItem, itemsKey);
                return addItem;
            }
            else
            {
                

                _unitOfWork.BeginCacheTransaction();

                await _unitOfWork.ExecuteInTransactionCacheAsync(async x =>
                {
                    var createdHeader = await _cartHeaderCacheRepository.CreateCartHeaderAsync(headerKey, new CartHeader(), TimeSpan.FromHours(24), x);
                    var addedItem = await _cartItemCacheRepository.AddItemAsync(mappedItem, itemsKey, x);
                });

                var commited = await _unitOfWork.CacheCommitTransactionAsync();



                return commited;
            }
        }

        public async Task<bool> DecreaseItemAsync(CartItemDto item, int userId)
        {
            if (!await _userService.IsUserExistsByIdAsync(userId))
            {
                return false;
            }

            var mappedItem = _mapper.Map<CartItem>(item);
            var itemsKey = $"{_cartSettings.CartItems}:{userId}";

            if (item.Count != 1)
            {
                return await _cartItemCacheRepository.DecreaseItemAsync(mappedItem, itemsKey);
            }

            else
            {
                return await _cartItemCacheRepository.RemoveItemAsync(mappedItem, itemsKey);
            }
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

        public async Task<bool> IncreaseItemAsync(CartItemDto item, int userId)
        {
            if (!await _userService.IsUserExistsByIdAsync(userId))
            {
                return false;
            }

            var itemsKey = $"{_cartSettings.CartItems}:{userId}";
            var mappedItem = _mapper.Map<CartItem>(item);

            var increasedItem = await _cartItemCacheRepository.IncreaseItemAsync(mappedItem, itemsKey);

            return increasedItem;
        }

        public async Task<bool> RemoveItemAsync(CartItemDto item, int userId)
        {
            if (!await _userService.IsUserExistsByIdAsync(userId))
            {
                return false;
            }

            var itemsKey = $"{_cartSettings.CartItems}:{userId}";
            var mappedItem = _mapper.Map<CartItem>(item);

            var deletedItem = await _cartItemCacheRepository.RemoveItemAsync(mappedItem, itemsKey);
            return deletedItem;
        }

        public async Task<bool> UpdateItemCountAsync(CartItemDto item, int userId, int newCount)
        {
            if (!await _userService.IsUserExistsByIdAsync(userId))
            {
                return false;
            }

            if(newCount < 0)
            {
                return false;
            }

            if(newCount == 0)
            {
                return await RemoveItemAsync(item, userId);
            }

            var itemsKey = $"{_cartSettings.CartItems}:{userId}";
            var mappedItem = _mapper.Map<CartItem>(item);

            var updatedItem = await _cartItemCacheRepository.UpdateItemCountAsync(mappedItem, itemsKey, newCount);
            return updatedItem;
        }
    }
}
