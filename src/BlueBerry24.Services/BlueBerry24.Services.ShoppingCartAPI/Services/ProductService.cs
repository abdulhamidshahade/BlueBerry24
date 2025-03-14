using BlueBerry24.Services.ShoppingCartAPI.Models.DTOs;
using BlueBerry24.Services.ShoppingCartAPI.Models.DTOs.DTOs;
using BlueBerry24.Services.ShoppingCartAPI.Services.Interfaces;
using Newtonsoft.Json;

namespace BlueBerry24.Services.ShoppingCartAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {

            var client = _httpClientFactory.CreateClient("Products");

            var response = await client.GetAsync($"/api/products");

            var apiContent = await response.Content.ReadAsStringAsync();

            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if (resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(resp.Data));
            }

            return new List<ProductDto>();
        }
    }
}
