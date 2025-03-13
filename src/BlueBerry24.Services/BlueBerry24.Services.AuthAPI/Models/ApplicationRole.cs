using Microsoft.AspNetCore.Identity;

namespace BlueBerry24.Services.AuthAPI.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
        {

        }

        public ApplicationRole(string role) : base(role)
        {

        }
    }
}
