using BlueBerry24.Application.Dtos.AuthDtos;

namespace BlueBerry24.Application.Services.Interfaces.AuthServiceInterfaces
{
    public interface IAuthService
    {
        Task<RegisterResponseDto> Register(RegisterRequestDto requestDto);
        Task<LoginResponseDto> Login(LoginRequestDto requestDto);
    }
}
