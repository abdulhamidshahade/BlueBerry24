using BlueBerry24.Domain.Entities.Stock;

namespace BlueBerry24.Domain.Repositories.StockInterfaces
{
    public interface IStockRepository
    {
        Task<Stock> CreateStockAsync(Stock stockDto);
        Task<bool> UpdateStockByIdAsync(string id, Stock stock);
        Task<bool> DeleteStockByIdAsync(string id);
        Task<Stock> GetStockByIdAsync(string id);
        Task<List<Stock>> GetStocksByShopIdAsync(string shopId);

        Task CheckStock(string productId, string shopId, int quantity);

        Task IncreaseByItemAsync(string productId, string shopId);
        Task DecreaseByItemAsync(string productId, string shopId);

        Task<bool> IsStockAvailableAsync(string productId, string shopId);

    }
}
