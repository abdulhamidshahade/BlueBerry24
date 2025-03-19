using BlueBerry24.Services.UserCouponAPI.Models.DTOs;
using BlueBerry24.Services.UserCouponAPI.Services.Interfaces;
using Newtonsoft.Json;

namespace BlueBerry24.Services.UserCouponAPI.Services
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> IsCouponExistsByIdAsync(string couponId)
        {
            var client = _httpClientFactory.CreateClient("Coupon");

            var request = await client.GetAsync($"api/coupons/exists/{couponId}");

            var apiContent = await request.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            return response.IsSuccess;
        }
    }
}
