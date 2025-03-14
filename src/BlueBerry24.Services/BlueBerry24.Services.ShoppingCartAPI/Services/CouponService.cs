using BlueBerry24.Services.ShoppingCartAPI.Services.Interfaces;

namespace BlueBerry24.Services.ShoppingCartAPI.Services
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;


        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public Task<bool> IsAvailableAsync(string userId)
        {
            var client = _httpClientFactory.CreateClient("Coupons");

            throw new NotImplementedException();
        }

        public Task<bool> IsUsedByUserAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
