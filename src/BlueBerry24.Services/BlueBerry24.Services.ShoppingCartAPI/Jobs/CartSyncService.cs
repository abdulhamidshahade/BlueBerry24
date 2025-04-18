
using BlueBerry24.Services.ShoppingCartAPI.Data;
using BlueBerry24.Services.ShoppingCartAPI.Services.Interfaces;

namespace BlueBerry24.Services.ShoppingCartAPI.Jobs
{
    public class CartSyncService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CartSyncService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var cacheService = scope.ServiceProvider.GetRequiredService<ICartCacheService>();
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var keys = await cacheService.GetAllActiveCartUserIdsAsync();

                    foreach(var key in keys)
                    {
                        var userId = ExtractUserIdFromKey(key);
                        var cart = await cacheService.GetCartAsync(userId);

                        if(cart != null)
                        {
                            await SyncCartToDatabaseAsync(cart, dbContext);
                        }
                    }
                }
                catch
                {

                }
                await Task.Delay(TimeSpan.FromMinutes(60), stoppingToken);
            }
        }

        private string ExtractUserIdFromKey(string key) => key.Replace("cart:", "");
    }
}
