using Microsoft.AspNetCore.Authorization;

namespace Berryfy.Application.Authorization.Requirements
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public string[] AllowedRoles { get; }

        public RoleRequirement(params string[] allowedRoles)
        {
            AllowedRoles = allowedRoles ?? throw new ArgumentNullException(nameof(allowedRoles));
        }
    }
}
