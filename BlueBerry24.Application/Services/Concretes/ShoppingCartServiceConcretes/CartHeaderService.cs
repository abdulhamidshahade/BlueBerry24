using AutoMapper;
using BlueBerry24.Application.Dtos.ShoppingCartDtos;
using BlueBerry24.Application.Services.Interfaces.AuthServiceInterfaces;
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
    public class CartHeaderService : ICartHeaderService
    {
        private readonly ICartHeaderRepository _cartHeaderRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public CartHeaderService(ICartHeaderRepository cartHeaderRepository,
                                 IMapper mapper,
                                 IUserService userService)
        {
            _cartHeaderRepository = cartHeaderRepository;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<CartHeaderDto> CreateCartHeaderAsync(int userId)
        {
            
            if(!await _userService.IsUserExistsByIdAsync(userId))
            {
                return null;
            }

            var createdCartHeader = await _cartHeaderRepository.CreateCartAsync(userId);

            return _mapper.Map<CartHeaderDto>(createdCartHeader);
        }

        public async Task<bool> DeleteCartHeaderAsync(int userId)
        {
            if (!await _userService.IsUserExistsByIdAsync(userId))
            {
                return false;
            }

            var deletedCartHeader = await _cartHeaderRepository.DeleteCartHeaderAsync(userId);

            return deletedCartHeader;
        }

        public async Task<bool> ExistsByIdAsync(int userId)
        {
            if (!await _userService.IsUserExistsByIdAsync(userId))
            {
                return false;
            }

            var isExists = await _cartHeaderRepository.ExistsByIdAsync(userId);

            return isExists;
        }

        public async Task<CartHeaderDto> GetCartHeaderAsync(int userId)
        {
            if (!await _userService.IsUserExistsByIdAsync(userId))
            {
                return null;
            }

            var cartHeaderDto = _mapper.Map<CartHeaderDto>(await _cartHeaderRepository.GetCartHeaderAsync(userId));

            return cartHeaderDto;
        }

        public async Task<CartHeaderDto> UpdateCartHeaderAsync(int userId, CartHeaderDto header)
        {
            if (!await _userService.IsUserExistsByIdAsync(userId))
            {
                return null;
            }

            var headerMapped = _mapper.Map<CartHeader>(header);

            var updatedCartHeader = await _cartHeaderRepository.UpdateCartHeaderAsync(userId, headerMapped);

            var headerDto = _mapper.Map<CartHeaderDto>(updatedCartHeader);

            return headerDto;
        }
    }
}
