using BlueBerry24.Domain.Entities.Auth;

namespace BlueBerry24.Domain.Repositories.Interfaces.Auth
{
    public interface IAuthRepository
    {
        Task<ApplicationUser> Register(ApplicationUser requestModel);
    }
}
