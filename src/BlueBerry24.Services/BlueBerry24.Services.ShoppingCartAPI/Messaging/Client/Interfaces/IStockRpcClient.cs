namespace BlueBerry24.Services.ShoppingCartAPI.Messaging.Client.Interfaces
{
    public interface IStockRpcClient : IDisposable
    {
        Task<bool> IsProductAvailableInStockAsync(string productId, string shopId);
    }
}
