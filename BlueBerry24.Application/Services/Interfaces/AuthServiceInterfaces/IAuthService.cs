using BlueBerry24.Application.Dtos;
using BlueBerry24.Application.Dtos.AuthDtos;

namespace BlueBerry24.Application.Services.Interfaces.AuthServiceInterfaces
{
    public interface IAuthService
    {
        Task<RegisterResponseDto> Register(RegisterRequestDto requestDto);
        Task<LoginResponseDto> Login(LoginRequestDto requestDto);
        Task<bool> ForgotPasswordAsync(ForgotPasswordRequestDto requestDto);
        Task<bool> ResetPasswordAsync(ResetPasswordDto requestDto);
        Task<bool> ConfirmEmailAsync(EmailConfirmationDto confirmationDto);
        Task<bool> ResendConfirmationEmailAsync(string email);
    }
}
