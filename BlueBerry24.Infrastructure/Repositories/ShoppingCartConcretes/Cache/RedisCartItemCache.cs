using BlueBerry24.Domain.Entities.ShoppingCart;
using BlueBerry24.Domain.Repositories.ShoppingCartInterfaces.Cache;
using StackExchange.Redis;

namespace BlueBerry24.Infrastructure.Repositories.ShoppingCartConcretes.Cache
{
    public class RedisCartItemCache : ICartItemCacheRepository
    {
        private IDatabase _db;


        public RedisCartItemCache(ConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task<bool> AddItemAsync(CartItem item, string key, ITransaction transaction)
        {
            //var addItems = await _db.HashIncrementAsync(key, item.Id.ToString(), 1);

            var addedItem = await transaction.HashIncrementAsync(key, item.Id.ToString(), 1);

            int quantity = (int)await _db.HashGetAsync(key, item.Id.ToString());

            return quantity > 0;
        }

        public async Task<bool> DecreaseItemAsync(CartItem item, string key)
        {
            int quantity = (int)await _db.HashGetAsync(key, item.Id.ToString());

            var decreasedItem = await _db.HashIncrementAsync(key, item.Id.ToString(), -1);

            return decreasedItem < quantity;
        }

        public async Task<bool> DeleteAllItems(string key, ITransaction transaction)
        {
            var deletedItems = await transaction.KeyDeleteAsync(key);

            return deletedItems;
        }

        public async Task<List<CartItem>> GetAllItems(string key)
        {
            HashEntry[] entries = await _db.HashGetAllAsync(key);

            List<CartItem> items = new List<CartItem>();

            foreach(var entry in entries)
            {
                items.Add(new CartItem
                {
                    ProductId = entry.Name,
                    Count = (int)entry.Value
                });
            }

            return items;
        }

        public async Task<bool> IncreaseItemAsync(CartItem item, string key)
        {
            int quantity = (int)await _db.HashGetAsync(key, item.Id.ToString());

            var increasedItem = await _db.HashIncrementAsync(key, item.Id.ToString(), 1);

            

            return increasedItem > quantity;
        }

        public async Task<bool> RemoveItemAsync(CartItem item, string key)
        {
            var deletedItem = await _db.HashDeleteAsync(key, item.Id.ToString());
            return deletedItem;
        }

        public async Task<bool> UpdateItemCountAsync(CartItem item, string key, int newCount)
        {
            int quantity = (int)await _db.HashGetAsync(key, item.Id.ToString());

            if(quantity == newCount)
            {
                return true;
            }

            var updatedItemCount = await _db.HashIncrementAsync(key, item.Id.ToString(), newCount - quantity);

            return updatedItemCount != quantity;
        }


    }
}
