using Berryfy.Domain.Constants;

namespace Berryfy.Application.Authorization.Attributes
{
    public class UserAndAboveAttribute : AuthorizeRolesAttribute
    {
        public UserAndAboveAttribute() : base(RoleConstants.SuperAdmin, RoleConstants.Admin, RoleConstants.User) { }
    }
}
