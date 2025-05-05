using BlueBerry24.Application.Services.Interfaces.AuthServiceInterfaces;
using BlueBerry24.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity;

namespace BlueBerry24.Application.Services.Concretes.AuthServiceConcretes
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

        public async Task<bool> IsUserExistsByIdAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            return user != null;
        }
    }
}
