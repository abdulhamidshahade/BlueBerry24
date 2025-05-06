using AutoMapper;
using BlueBerry24.Application.Dtos.ShoppingCartDtos;
using BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces;
using BlueBerry24.Domain.Repositories.ShoppingCartInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Services.Concretes.ShoppingCartServiceConcretes
{
    class CartHeaderService : ICartHeaderService
    {
        private readonly ICartHeaderRepository _cartHeaderRepository;
        private readonly IMapper _mapper;

        public CartHeaderService(ICartHeaderRepository cartHeaderRepository,
            IMapper mapper)
        {
            _cartHeaderRepository = cartHeaderRepository;
            _mapper = mapper;
        }

        public async Task<CartHeaderDto> CreateCartAsync(int userId)
        {
            var createdHeader = await _cartHeaderRepository.CreateCartAsync(userId);

            if(createdHeader == null)
            {
                return null;
            }

            return _mapper.Map<CartHeaderDto>(createdHeader);
        }

        public Task<bool> DeleteCartHeaderAsync(int id)
        {
            
        }

        public Task<bool> ExistsByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CartHeaderDto> UpdateCartHeaderAsync(int id, CartHeaderDto header)
        {
            throw new NotImplementedException();
        }
    }
}
