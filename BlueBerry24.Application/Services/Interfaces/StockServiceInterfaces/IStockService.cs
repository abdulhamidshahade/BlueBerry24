using BlueBerry24.Application.Dtos.StockDtos;
using BlueBerry24.Domain.Entities.Stock;

namespace BlueBerry24.Application.Services.Interfaces.StockServiceInterfaces
{
    public interface IStockService
    {
        Task<StockDto> CreateStockAsync(CreateStockDto stockDto);
        Task<bool> UpdateStockByIdAsync(int id, UpdateStockDto stockDto);
        Task<bool> DeleteStockByIdAsync(int id);
        Task<StockDto> GetStockByIdAsync(int id);
        //Task<List<StockDto>> GetStocksByShopIdAsync(int shopId);

        //Task CheckStock(int productId, int quantity);

        Task<bool> IncreaseByItemAsync(int productId);
        Task<bool> DecreaseByItemAsync(int productId);

        Task<bool> IsStockAvailableAsync(int productId);
        //Task<StockDto> GetStockByProductId(int productId);
    }
}
