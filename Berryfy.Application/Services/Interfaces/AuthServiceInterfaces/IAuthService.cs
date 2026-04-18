using Berryfy.API.Controllers;
using Berryfy.Application.Dtos;
using Berryfy.Application.Dtos.AuthDtos;

namespace Berryfy.Application.Services.Interfaces.AuthServiceInterfaces
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
