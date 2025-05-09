using Microsoft.AspNetCore.Identity;

namespace BlueBerry24.Domain.Entities.AuthEntities
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
