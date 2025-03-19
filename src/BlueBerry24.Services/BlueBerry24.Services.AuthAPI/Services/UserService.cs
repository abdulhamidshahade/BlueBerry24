using BlueBerry24.Services.AuthAPI.Models;
using BlueBerry24.Services.AuthAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BlueBerry24.Services.AuthAPI.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<bool> IsUserExistsByEmailAsync(string emailAddress)
        {
            var user = await _userManager.FindByEmailAsync(emailAddress);

            return user != null;
        }

        public async Task<bool> IsUserExistsByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return user != null;
        }
    }
}
