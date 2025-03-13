using Microsoft.AspNetCore.Identity;

namespace BlueBerry24.Services.AuthAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
