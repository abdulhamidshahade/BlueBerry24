using BlueBerry24.Domain.Entities.StockEntities;

namespace BlueBerry24.Domain.Repositories.StockInterfaces
{
    public interface IStockRepository
    {
        Task<Stock> CreateStockAsync(Stock stockDto);
        Task<bool> UpdateStockByIdAsync(int id, Stock stock);
        Task<bool> DeleteStockByIdAsync(Stock stock);
        Task<Stock> GetStockByIdAsync(int id);
        //Task<List<Stock>> GetStocksByShopIdAsync(int shopId);

        Task CheckStock(int productId, int shopId, int quantity);

        //Task<bool> IncreaseByItemAsync(int productId, int shopId);
        //Task<bool> DecreaseByItemAsync(int productId, int shopId);

        Task<bool> UpdateStockAsync(Stock stock);

        Task<bool> IsStockAvailableAsync(int productId);
        Task<Stock> GetStockByProductId(int productId);
        Task<List<Stock>> GetStocks();
    }
}
