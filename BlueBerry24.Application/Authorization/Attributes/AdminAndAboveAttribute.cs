using BlueBerry24.Domain.Constants;

namespace BlueBerry24.Application.Authorization.Attributes
{
    public class AdminAndAboveAttribute : AuthorizeRolesAttribute
    {
        public AdminAndAboveAttribute() : base(RoleConstants.SuperAdmin, RoleConstants.Admin) { }
    }
}
