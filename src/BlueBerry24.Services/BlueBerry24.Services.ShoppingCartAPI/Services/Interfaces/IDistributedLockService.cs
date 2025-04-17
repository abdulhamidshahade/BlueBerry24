namespace BlueBerry24.Services.ShoppingCartAPI.Services.Interfaces
{
    public interface IDistributedLockService
    {
        Task<bool> AcquireLockAsync(string key, string value, TimeSpan expiry);
        Task<bool> ReleaseLockAsync(string key, string value);
    }
}
