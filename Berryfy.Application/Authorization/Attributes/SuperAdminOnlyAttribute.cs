using Berryfy.Domain.Constants;

namespace Berryfy.Application.Authorization.Attributes
{
    public class SuperAdminOnlyAttribute : AuthorizeRolesAttribute
    {
        public SuperAdminOnlyAttribute() : base(RoleConstants.SuperAdmin) { }
    }
}
