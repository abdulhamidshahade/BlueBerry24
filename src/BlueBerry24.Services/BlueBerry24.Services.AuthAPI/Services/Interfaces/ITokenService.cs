using BlueBerry24.Services.AuthAPI.Models;

namespace BlueBerry24.Services.AuthAPI.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateToken(ApplicationUser user);
    }
}
