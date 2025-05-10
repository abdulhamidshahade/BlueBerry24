using AutoMapper;
using BlueBerry24.Application.Dtos.ShopDtos;
using BlueBerry24.Application.Services.Interfaces.ShopServiceInterfaces;
using BlueBerry24.Domain.Entities.ShopEntities;
using BlueBerry24.Domain.Repositories.ShopInterfaces;

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

        public async Task<ShopDto> GetByIdAsync(int id)
        {
            var shop = await _shopRepository.GetByIdAsync(id);

            if (shop == null)
            {
                return null;
            }

            return _mapper.Map<ShopDto>(shop);
        }

        public async Task<ShopDto> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            var shop = await _shopRepository.GetByNameAsync(name);
            
            if (shop == null)
            {
                return null;
            }

            return _mapper.Map<ShopDto>(shop);
        }

        public async Task<IEnumerable<ShopDto>> GetAllAsync()
        {
            var shops = await _shopRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ShopDto>>(shops);
        }

        public async Task<ShopDto> CreateAsync(CreateShopDto shopDto)
        {
            if (shopDto == null)
            {
                return null;
            }

            if (await ExistsByNameAsync(shopDto.Name))
            {
                return null;
            }

            var shop = _mapper.Map<Shop>(shopDto);
            var createdShop = await _shopRepository.CreateAsync(shop);

            return _mapper.Map<ShopDto>(createdShop);
        }


        public async Task<ShopDto> UpdateAsync(int id, UpdateShopDto shopDto)
        {
            if (shopDto == null)
            {
                return null;
            }

            //var existingShop = await _shopRepository.GetByIdAsync(id);
            //if (existingShop == null)
            //{
            //    throw new NotFoundException($"Shop with id: {id} not found");
            //}

            //var shopWithSameName = await _shopRepository.GetAsync(c => c.Name == shopDto.Name && c.Id != id);

            //if (shopWithSameName != null)
            //{
            //    throw new DuplicateEntityException($"Shop with name: {shopDto.Name} already exists");
            //}

            //existingShop.Name = shopDto.Name;
            //existingShop.Description = shopDto.Description;
            //existingShop.Email = shopDto.Email;
            //existingShop.Phone = shopDto.Phone;
            //existingShop.Address = shopDto.Address;
            //existingShop.Country = shopDto.Country;
            //existingShop.City = shopDto.City;
            //existingShop.LogoUrl = shopDto.LogoUrl;
            //existingShop.Status = shopDto.Status;

            var mappedShop = _mapper.Map<Shop>(shopDto);

            var updatedShop = await _shopRepository.UpdateAsync(id, mappedShop);
            return _mapper.Map<ShopDto>(updatedShop);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var shop = await _shopRepository.GetByIdAsync(id);
            if (shop == null)
            {
                return false;
            }

            var deletedShop = await _shopRepository.DeleteAsync(shop);
            return deletedShop;
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _shopRepository.ExistsByIdAsync(id);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            return await _shopRepository.ExistsByNameAsync(name);
        }
    }
}
