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

        public async Task<CartDto> GetCartAsync(string userId)
        {
            var cartJson = await _db.StringGetAsync($"cart:{userId}");

            return cartJson.IsNullOrEmpty ? null : JsonConvert.DeserializeObject<CartDto>(cartJson);
        }

        public async Task SetCartAsync(string userId, CartDto cart)
        {
            var cartJson = JsonConvert.SerializeObject(cart);

            await _db.StringSetAsync($"cart:{userId}", cartJson, TimeSpan.FromMinutes(60));
        }
    }
}
