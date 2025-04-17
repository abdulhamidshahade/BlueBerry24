using BlueBerry24.Services.ShoppingCartAPI.Services.Interfaces;

using StackExchange.Redis;

namespace BlueBerry24.Services.ShoppingCartAPI.Services
{
    public class RedisDistributedLockService : IDistributedLockService
    {
        private readonly IDatabase _db;

        public RedisDistributedLockService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }
        public async Task<bool> AcquireLockAsync(string key, string value, TimeSpan expiry)
        {
            return await _db.LockTakeAsync(key, value, expiry);
        }

        public async Task<bool> ReleaseLockAsync(string key, string value)
        {
            return await _db.LockReleaseAsync(key, value);
        }
    }
}
