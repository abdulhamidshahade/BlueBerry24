using BlueBerry24.Domain.Entities.ShoppingCartEntities;
using BlueBerry24.Domain.Repositories.ShoppingCartInterfaces.Cache;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace BlueBerry24.Infrastructure.Repositories.ShoppingCartConcretes.Cache
{
    class RedisCartHeaderCache : ICartHeaderCacheRepository
    {
        private readonly IDatabase _db;

        public RedisCartHeaderCache(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task<bool> CreateCartHeaderAsync(string key, 
                                                      CartHeader cartHeader, 
                                                      TimeSpan timeSpan,
                                                      ITransaction? transaction = null)
        {
            //var key = $"cart-header:{userId}";
            //var cartHeader = new CartHeader
            //{
            //    IsActive = true
            //};


            var cartToJson = JsonConvert.SerializeObject(cartHeader);

            var setHeaderSuccess = await transaction.StringSetAsync(key, cartToJson, timeSpan);
            var AddUserSuccess = await transaction.SetAddAsync("active_cart_users", cartHeader.UserId);

            return setHeaderSuccess && setHeaderSuccess;
        }

        public async Task<bool> DeleteCartHeaderAsync(int userId, string key, ITransaction transaction)
        {
            //var key = $"cart-header:{userId}";

            //var deletedCartHeader = await _db.KeyDeleteAsync(key);
            //var deletedActiveCart = await _db.SetRemoveAsync("active_cart_users", userId);

            var headerDeleted = await transaction.KeyDeleteAsync(key);
            var activeCartDeleted = await transaction.SetRemoveAsync("active_cart_users", userId);

            return headerDeleted && activeCartDeleted;
        }

        public Task<bool> ExistsByUserIdAsync(string key)
        {
            //var key = $"cart-header:{userId}";

            var isExists = _db.KeyExistsAsync(key);
            return isExists;
        }

        public async Task<CartHeader> GetCartHeaderAsync(string key)
        {
            //var key = $"cart-header:{userId}";

            var cartHeaderJson = await _db.StringGetAsync(key);

            if(!string.IsNullOrEmpty(cartHeaderJson))
            {
                var cartHeaderObj = JsonConvert.DeserializeObject<CartHeader>(cartHeaderJson);

                return cartHeaderObj;
            }


            return null;
            
        }

        public async Task<bool> UpdateCartHeaderAsync(string key, CartHeader cartHeader)
        {
            //var key = $"cart-header:{userId}";

            //var deletedCartHeader = await _db.KeyDeleteAsync(key);

            //if (deletedCartHeader)
            //{
            //    var cartHeaderJson = JsonConvert.SerializeObject(cartHeader);

            //    var updatedCart = await _db.StringSetAsync()
            //}

            var cartHeaderJson = JsonConvert.SerializeObject(cartHeader);
            var updatedCartHeader = await _db.StringSetAsync(key, cartHeaderJson);

            return updatedCartHeader;
        }
    }
}
