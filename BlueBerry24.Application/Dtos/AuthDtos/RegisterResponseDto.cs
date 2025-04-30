namespace BlueBerry24.Application.Dtos.AuthDtos
{
    public class RegisterResponseDto
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public ApplicationUserDto User { get; set; }
    }
}
