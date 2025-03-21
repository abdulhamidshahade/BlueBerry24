namespace BlueBerry24.Services.StockAPI.Services.Interfaces
{
    public interface IShopService
    {
        Task<bool> ExistByIdAsync(string shopId);
    }
}
