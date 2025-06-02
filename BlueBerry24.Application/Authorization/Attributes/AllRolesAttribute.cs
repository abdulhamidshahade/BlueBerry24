using BlueBerry24.Domain.Constants;

namespace BlueBerry24.Application.Authorization.Attributes
{
    public class AllRolesAttribute : AuthorizeRolesAttribute
    {
        public AllRolesAttribute() : base(RoleConstants.SuperAdmin, RoleConstants.Admin, RoleConstants.User) { }
    }
}
