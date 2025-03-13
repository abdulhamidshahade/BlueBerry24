using BlueBerry24.Services.ShopAPI.Models.DTOs.ShopDtos;

namespace BlueBerry24.Services.ShopAPI.Services.Interfaces
{
    public interface IShopService
    {
        Task<ShopDto> GetByIdAsync(string id);
        Task<ShopDto> GetByNameAsync(string name);
        Task<IEnumerable<ShopDto>> GetAllAsync();
        Task<ShopDto> CreateAsync(CreateShopDto shopDto);
        Task<ShopDto> UpdateAsync(string id, UpdateShopDto shopDto);
        Task<bool> DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<bool> ExistsByNameAsync(string name);
    }
}
