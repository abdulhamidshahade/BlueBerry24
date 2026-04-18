using System.ComponentModel.DataAnnotations;

namespace Berryfy.Application.Dtos.AuthDtos
{
    public class ForgotPasswordRequestDto
    {
        public string Email { get; set; }
    }
}
