using BlueBerry24.Services.StockAPI.Models.DTOs;
using BlueBerry24.Services.StockAPI.Services.Interfaces;
using Newtonsoft.Json;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BlueBerry24.Services.StockAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> ExistsByShopIdAsync(string productId, string shopId)
        {
            var client = _httpClientFactory.CreateClient("Product");

            var request = await client.GetAsync($"/api/products/exists/shop/{productId}?shopId={shopId}");

            var apiContent = await request.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            return response.IsSuccess;
        }
    }
}
