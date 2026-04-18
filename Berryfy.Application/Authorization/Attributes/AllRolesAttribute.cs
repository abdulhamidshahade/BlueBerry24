using Berryfy.Domain.Constants;

namespace Berryfy.Application.Authorization.Attributes
{
    public class AllRolesAttribute : AuthorizeRolesAttribute
    {
        public AllRolesAttribute() : base(RoleConstants.SuperAdmin, RoleConstants.Admin, RoleConstants.User) { }
    }
}
