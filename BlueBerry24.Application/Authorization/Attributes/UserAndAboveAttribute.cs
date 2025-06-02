using BlueBerry24.Domain.Constants;

namespace BlueBerry24.Application.Authorization.Attributes
{
    public class UserAndAboveAttribute : AuthorizeRolesAttribute
    {
        public UserAndAboveAttribute() : base(RoleConstants.SuperAdmin, RoleConstants.Admin, RoleConstants.User) { }
    }
}
