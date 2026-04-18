using Microsoft.AspNetCore.Identity;

namespace Berryfy.Domain.Entities.AuthEntities
{ 
    public class ApplicationRole : IdentityRole<int>
    {
        public ApplicationRole()
        {

        }

        public ApplicationRole(string role) : base(role)
        {

        }
    }
}
