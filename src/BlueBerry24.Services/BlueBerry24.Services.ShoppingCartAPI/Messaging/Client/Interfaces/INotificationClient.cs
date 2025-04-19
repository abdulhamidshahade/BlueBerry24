namespace BlueBerry24.Services.ShoppingCartAPI.Messaging.Client.Interfaces
{
    public interface INotificationClient : IDisposable
    {
        Task NotifyShopByProductIncreament(string shopId, string productId, int count);
    }
}
