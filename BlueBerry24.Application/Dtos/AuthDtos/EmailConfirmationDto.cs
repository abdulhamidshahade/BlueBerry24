using System.ComponentModel.DataAnnotations;

namespace BlueBerry24.Application.Dtos.AuthDtos
{
    public class EmailConfirmationDto
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
