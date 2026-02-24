using BlueBerry24.API.Controllers;
using BlueBerry24.Application.Dtos;
using BlueBerry24.Application.Dtos.AuthDtos;

namespace BlueBerry24.Application.Services.Interfaces.AuthServiceInterfaces
{
    public interface IAuthService
    {
        Task<Result<RegisterResponseDto>> Register(RegisterRequestDto requestDto);
        Task<LoginResponseDto> Login(LoginRequestDto requestDto);
        Task<bool> ForgotPasswordAsync(ForgotPasswordRequestDto requestDto);
        Task<bool> ResetPasswordAsync(ResetPasswordRequestDto requestDto);
        Task<bool> ConfirmEmailAsync(EmailConfirmationDto confirmationDto);
        Task<bool> ResendConfirmationEmailAsync(string email);
    }
}
