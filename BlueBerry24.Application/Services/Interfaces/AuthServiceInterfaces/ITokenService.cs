

using BlueBerry24.Domain.Entities.Auth;

namespace BlueBerry24.Application.Services.Interfaces.AuthServiceInterfaces
{
    public interface ITokenService
    {
        Task<string> GenerateToken(ApplicationUser user);
    }
}
