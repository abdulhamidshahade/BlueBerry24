using BlueBerry24.Services.ShoppingCartAPI.Models.DTOs;
using BlueBerry24.Services.ShoppingCartAPI.Services.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace BlueBerry24.Services.ShoppingCartAPI.Services
{
    public class RedisCartCacheService : ICartCacheService
    {
        private readonly IDatabase _db;

        public RedisCartCacheService(ConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task DeleteCartAsync(string userId)
        {
            var key = $"cart:{userId}";

            await _db.KeyDeleteAsync(key);

            await _db.SetRemoveAsync("active_cart_users", userId);
        }

        public async Task<IEnumerable<string>> GetAllActiveCartUserIdsAsync()
        {
            var userIds = await _db.SetMembersAsync("active_cart_users");
            return userIds.Select(x => x.ToString());
        }

        public async Task<CartDto> GetCartAsync(string userId)
        {
            var cartJson = await _db.StringGetAsync($"cart:{userId}");

            return cartJson.IsNullOrEmpty ? null : JsonConvert.DeserializeObject<CartDto>(cartJson);
        }

        public async Task SetCartAsync(string userId, CartDto cart, TimeSpan timeSpan)
        {
            var key = $"cart:{userId}";
            var cartJson = JsonConvert.SerializeObject(cart);


            await _db.StringSetAsync(key, cartJson, timeSpan);

            await _db.SetAddAsync("active_cart_users", userId);
        }
    }
}
