using BlueBerry24.Domain.Constants;

namespace BlueBerry24.Application.Authorization.Attributes
{
    public class SuperAdminOnlyAttribute : AuthorizeRolesAttribute
    {
        public SuperAdminOnlyAttribute() : base(RoleConstants.SuperAdmin) { }
    }
}
