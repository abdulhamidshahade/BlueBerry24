using Berryfy.Domain.Constants;

namespace Berryfy.Application.Authorization.Attributes
{
    public class AdminAndAboveAttribute : AuthorizeRolesAttribute
    {
        public AdminAndAboveAttribute() : base(RoleConstants.SuperAdmin, RoleConstants.Admin) { }
    }
}
