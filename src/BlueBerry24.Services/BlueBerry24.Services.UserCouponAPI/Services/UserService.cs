using BlueBerry24.Services.UserCouponAPI.Models.DTOs;
using BlueBerry24.Services.UserCouponAPI.Services.Interfaces;
using Newtonsoft.Json;

namespace BlueBerry24.Services.UserCouponAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> IsUserExistsByEmailAsync(string emailAddress)
        {
            var client = _httpClientFactory.CreateClient("User");

            var request = await client.GetAsync($"api/auth/exists/email-address/{emailAddress}");

            var apiContent = await request.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            return response.IsSuccess;
        }

        public async Task<bool> IsUserExistsByIdAsync(string userId)
        {
            var client = _httpClientFactory.CreateClient("User");

            var request = await client.GetAsync($"api/auth/exists/{userId}");

            var apiContent = await request.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            return response.IsSuccess;
        }
    }
}
