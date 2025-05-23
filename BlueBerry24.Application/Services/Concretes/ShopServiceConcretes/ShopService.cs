using AutoMapper;
using BlueBerry24.Application.Dtos.ShopDtos;
using BlueBerry24.Application.Services.Interfaces.ShopServiceInterfaces;
using BlueBerry24.Domain.Entities.ShopEntities;
using BlueBerry24.Domain.Repositories.ShopInterfaces;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Application.Services.Concretes.ShopServiceConcretes
{
    public class ShopService : IShopService
    {
        private readonly IShopRepository _shopRepository;
        private readonly IMapper _mapper;

        public ShopService(IShopRepository shopRepository,
                           IMapper mapper)
        {
            _shopRepository = shopRepository;
            _mapper = mapper;
        }

        public async Task<Shop> GetShopAsync(int id)
        {
            if(id <= 0)
            {
                return null;
            }

            var shop = await _shopRepository.GetShopAsync(id);

            if(shop == null)
            {
                return null;
            }

            return shop;
        }

        public async Task<ShopDto> UpdateShopAsync(int id, UpdateShopDto shop)
        {

            if(shop == null || id <= 0)
            {
                return null;
            }

            var existingShop = await _shopRepository.GetShopAsync(id);

            if(existingShop == null)
            {
                return null;
            }

            var mappedShop = _mapper.Map<Shop>(shop);

            var updatedShop = await _shopRepository.UpdateShopAsync(mappedShop);

            if(updatedShop != null)
            {
                return _mapper.Map<ShopDto>(updatedShop);
            }

            return null;
        }
    }
}
