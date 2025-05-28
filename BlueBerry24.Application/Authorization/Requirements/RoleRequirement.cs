using Microsoft.AspNetCore.Authorization;

namespace BlueBerry24.Application.Authorization.Requirements
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
