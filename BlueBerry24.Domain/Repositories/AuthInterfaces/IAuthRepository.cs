using BlueBerry24.Domain.Entities.Auth;

namespace BlueBerry24.Domain.Repositories.AuthInterfaces
{
    public interface IAuthRepository
    {
        Task<ApplicationUser> Register(ApplicationUser requestModel);
    }
}
