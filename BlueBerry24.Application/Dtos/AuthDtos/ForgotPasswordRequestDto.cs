using System.ComponentModel.DataAnnotations;

namespace BlueBerry24.Application.Dtos.AuthDtos
{
    public class ForgotPasswordRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
