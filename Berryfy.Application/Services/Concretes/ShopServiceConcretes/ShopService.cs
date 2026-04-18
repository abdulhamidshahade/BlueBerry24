using AutoMapper;
using Berryfy.Application.Dtos.ShopDtos;
using Berryfy.Application.Services.Interfaces.ShopServiceInterfaces;
using Berryfy.Domain.Entities.ShopEntities;
using Berryfy.Domain.Repositories.ShopInterfaces;

namespace Berryfy.Application.Services.Concretes.ShopServiceConcretes
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
