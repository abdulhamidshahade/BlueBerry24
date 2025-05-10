

using BlueBerry24.Domain.Entities.AuthEntities;

namespace BlueBerry24.Application.Services.Interfaces.AuthServiceInterfaces
{
    public interface ITokenService
    {
        Task<string> GenerateToken(ApplicationUser user);
    }
}
