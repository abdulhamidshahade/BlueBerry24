using AutoMapper;
using BlueBerry24.Services.ShopAPI.Services.Interfaces;
using BlueBerry24.Services.ShopAPI.Models;
using BlueBerry24.Services.ShopAPI.Services.Generic;
using BlueBerry24.Services.ShopAPI.Models.DTOs.ShopDtos;
using BlueBerry24.Services.ShopAPI.Exceptions;

namespace BlueBerry24.Services.ShopAPI.Services
{
    public class ShopService : IShopService
    {
        private readonly IRepository<Shop> _shopRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ShopService(IRepository<Shop> shopRepository, IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _shopRepository = shopRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ShopDto> GetByIdAsync(string id)
        {
            var shop = await _shopRepository.GetByIdAsync(id);

            if (shop == null)
            {
                throw new NotFoundException($"Shop with id: {id} not found");
            }

            return _mapper.Map<ShopDto>(shop);
        }

        public async Task<ShopDto> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"Shop with name: {name} cannot be empty", nameof(name));
            }

            var shop = await _shopRepository.GetAsync(c => c.Name == name);
            
            if (shop == null)
            {
                throw new NotFoundException($"Shop with name: {name} not found");
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
                throw new ArgumentNullException(nameof(shopDto));
            }

            if (await ExistsByNameAsync(shopDto.Name))
            {
                throw new DuplicateEntityException($"Shop with name {shopDto.Name} already exists");
            }

            var shop = _mapper.Map<Shop>(shopDto);
            await _shopRepository.AddAsync(shop);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ShopDto>(shop);
        }


        public async Task<ShopDto> UpdateAsync(string id, UpdateShopDto shopDto)
        {
            if (shopDto == null)
            {
                throw new ArgumentNullException(nameof(ShopDto));
            }

            var existingShop = await _shopRepository.GetByIdAsync(id);
            if (existingShop == null)
            {
                throw new NotFoundException($"Shop with id: {id} not found");
            }

            var shopWithSameName = await _shopRepository.GetAsync(c => c.Name == shopDto.Name && c.Id != id);

            if (shopWithSameName != null)
            {
                throw new DuplicateEntityException($"Shop with name: {shopDto.Name} already exists");
            }

            existingShop.Name = shopDto.Name;
            existingShop.Description = shopDto.Description;
            existingShop.Email = shopDto.Email;
            existingShop.Phone = shopDto.Phone;
            existingShop.Address = shopDto.Address;
            existingShop.Country = shopDto.Country;
            existingShop.City = shopDto.City;
            existingShop.LogoUrl = shopDto.LogoUrl;
            existingShop.Status = shopDto.Status;


            _shopRepository.Update(existingShop);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ShopDto>(existingShop);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var shop = await _shopRepository.GetByIdAsync(id);
            if (shop == null)
            {
                return false;
            }

            _shopRepository.Delete(shop);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _shopRepository.ExistsAsync(i => i.Id == id);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Shop name cannot be empty", nameof(name));
            }

            return await _shopRepository.ExistsAsync(c => c.Name == name);
        }
    }
}
