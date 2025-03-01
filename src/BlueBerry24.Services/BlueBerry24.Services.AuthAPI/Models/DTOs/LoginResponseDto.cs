namespace BlueBerry24.Services.AuthAPI.Models.DTOs
{
    public class LoginResponseDto
    {
        public ApplicationUserDto User { get; set; }
        public string Token { get; set; }
    }
}
