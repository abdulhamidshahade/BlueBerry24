using System.ComponentModel.DataAnnotations;

namespace BlueBerry24.Application.Dtos.AuthDtos
{
    public class CreateUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public bool EmailConfirmed { get; set; } = false;

        public List<string> Roles { get; set; } = new List<string> { "User" };
    }
}