namespace BlueBerry24.Services.StockAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<bool> ExistsByShopIdAsync(string productId, string shopId);
    }
}
