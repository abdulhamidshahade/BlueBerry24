using System.ComponentModel.DataAnnotations;

namespace BlueBerry24.Application.Dtos.AuthDtos
{
    public class ResetPasswordRequestDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
