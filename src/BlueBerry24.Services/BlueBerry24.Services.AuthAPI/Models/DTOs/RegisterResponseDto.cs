namespace BlueBerry24.Services.AuthAPI.Models.DTOs
{
    public class RegisterResponseDto
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public ApplicationUserDto User { get; set; }
    }
}
