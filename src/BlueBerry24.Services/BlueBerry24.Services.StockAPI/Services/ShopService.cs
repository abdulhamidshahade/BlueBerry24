using BlueBerry24.Services.StockAPI.Models.DTOs;
using BlueBerry24.Services.StockAPI.Services.Interfaces;
using Newtonsoft.Json;

namespace BlueBerry24.Services.StockAPI.Services
{
    public class ShopService : IShopService
    {
        private readonly IHttpClientFactory _httpClientFactor;

        public ShopService(IHttpClientFactory httpClientFactor)
        {
            _httpClientFactor = httpClientFactor;
        }

        public async Task<bool> ExistByIdAsync(string shopId)
        {
            var client = _httpClientFactor.CreateClient("Shop");

            var request = await client.GetAsync($"/api/shops/exists/{shopId}");

            var apiContent = await request.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            return response.IsSuccess;
        }
    }
}
