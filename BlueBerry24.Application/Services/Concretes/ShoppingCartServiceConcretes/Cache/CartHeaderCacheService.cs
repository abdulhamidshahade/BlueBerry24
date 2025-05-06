using AutoMapper;
using BlueBerry24.Application.Config.Settings;
using BlueBerry24.Application.Dtos.ShoppingCartDtos;
using BlueBerry24.Application.Services.Interfaces.AuthServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces.Cache;
using BlueBerry24.Domain.Entities.ShoppingCart;
using BlueBerry24.Domain.Repositories;
using BlueBerry24.Domain.Repositories.ShoppingCartInterfaces.Cache;
using Microsoft.Extensions.Options;

namespace BlueBerry24.Application.Services.Concretes.ShoppingCartServiceConcretes.Cache
{
    public class CartHeaderCacheService : ICartHeaderCacheService
    {
        private readonly ICartHeaderCacheRepository _cartHeaderCacheRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartItemCacheService _cartItemCacheService;
        private readonly ICartItemCacheRepository _cartItemCacheRepository;
        private readonly CartSettings _cartSettings;

        public CartHeaderCacheService(ICartHeaderCacheRepository cartHeaderCacheRepository,
                                      IMapper mapper,
                                      IUserService userService,
                                      IUnitOfWork unitOfWork,
                                      ICartItemCacheService cartItemCacheService,
                                      ICartItemCacheRepository cartItemCacheRepository,
                                      IOptions<CartSettings> options)
        {
            _cartHeaderCacheRepository = cartHeaderCacheRepository;
            _mapper = mapper;
            _userService = userService;
            _unitOfWork = unitOfWork;
            _cartItemCacheService = cartItemCacheService;
            _cartItemCacheRepository = cartItemCacheRepository;
            _cartSettings = options.Value;
        }

        public async Task<bool> CreateCartHeaderAsync(int userId, TimeSpan timeSpan)
        {
            if(timeSpan.TotalHours < 24 || timeSpan.TotalHours > 48)
            {
                return false;
            }

            if (!await _userService.IsUserExistsByIdAsync(userId))
            {
                return false;
            }

            var key = $"{_cartSettings.CartHeader}:{userId}";

            var createdHeader = await _cartHeaderCacheRepository.CreateCartHeaderAsync(userId, key, timeSpan);
            return createdHeader;
        }

        public async Task<bool> DeleteCartHeaderAsync(int userId)
        {
            if (!await _userService.IsUserExistsByIdAsync(userId))
            {
                return false;
            }

            _unitOfWork.BeginCacheTransaction();

            bool headerQueued = false;
            bool itemsQueued = false;

            var headerKey = $"{_cartSettings.CartHeader}:{userId}";
            var itemsKey = $"{_cartSettings.CartItems}:{userId}";

            await _unitOfWork.ExecuteInTransactionCacheAsync(async queue =>
            {
                headerQueued = await _cartHeaderCacheRepository.DeleteCartHeaderAsync(userId, headerKey, queue);
                itemsQueued = await _cartItemCacheRepository.DeleteAllItems(itemsKey, queue);
            });

            var commited =  await _unitOfWork.CacheCommitTransactionAsync();

            return headerQueued && itemsQueued && commited;  
        }

        public async Task<bool> ExistsByUserIdAsync(int userId)
        {
            if (!await _userService.IsUserExistsByIdAsync(userId))
            {
                return false;
            }

            var headerKey = $"{_cartSettings.CartHeader}:{userId}";

            var isExists = await _cartHeaderCacheRepository.ExistsByUserIdAsync(headerKey);

            return isExists;
        }

        public async Task<CartHeaderDto> GetCartHeaderAsync(int userId)
        {
            if (!await _userService.IsUserExistsByIdAsync(userId))
            {
                return null;
            }

            var headerKey = $"{_cartSettings.CartHeader}:{userId}";

            var cartHeader = await _cartHeaderCacheRepository.GetCartHeaderAsync(headerKey);

            return _mapper.Map<CartHeaderDto>(cartHeader);
        }

        public async Task<bool> UpdateCartHeaderAsync(int userId, CartHeaderDto cartHeader)
        {
            if (!await _userService.IsUserExistsByIdAsync(userId))
            {
                return false;
            }

            var mappedCart = _mapper.Map<CartHeader>(cartHeader);

            var headerKey = $"{_cartSettings.CartHeader}:{userId}";

            var updatedCartHeader = await _cartHeaderCacheRepository.UpdateCartHeaderAsync(headerKey, mappedCart);

            return updatedCartHeader;
        }
    }
}
