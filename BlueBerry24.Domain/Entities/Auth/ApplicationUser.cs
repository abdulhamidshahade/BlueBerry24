using Microsoft.AspNetCore.Identity;

namespace BlueBerry24.Domain.Entities.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
