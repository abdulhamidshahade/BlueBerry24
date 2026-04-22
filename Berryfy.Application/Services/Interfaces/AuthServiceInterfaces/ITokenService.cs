

using Berryfy.Domain.Entities.AuthEntities;

namespace Berryfy.Application.Services.Interfaces.AuthServiceInterfaces
{
    public interface ITokenService
    {
        Task<string> GenerateToken(ApplicationUser user);
        Task<string> GenerateRefreshToken(ApplicationUser user);
    }
}
