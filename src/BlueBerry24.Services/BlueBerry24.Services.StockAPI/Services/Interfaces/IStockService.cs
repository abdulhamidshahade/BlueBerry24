using BlueBerry24.Services.StockAPI.Models;
using BlueBerry24.Services.StockAPI.Models.DTOs;

namespace BlueBerry24.Services.StockAPI.Services.Interfaces
{
    public interface IStockService
    {
        Task<Stock> CreateStockAsync(CreateStockDto stockDto);
        Task<bool> UpdateStockByIdAsync(string id, StockDto stockDto);
        Task<bool> DeleteStockByIdAsync(string id);
        Task<Stock> GetStockByIdAsync(string id);
        Task<List<Stock>> GetStocksByShopIdAsync(string shopId);

        Task CheckStock(string productId, string shopId, int quantity);

        Task IncreaseByItemAsync(string productId, string shopId);
        Task DecreaseByItemAsync(string productId, string shopId);

    }
}
