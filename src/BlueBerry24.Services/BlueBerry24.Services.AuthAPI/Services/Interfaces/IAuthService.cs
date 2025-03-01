using BlueBerry24.Services.AuthAPI.Models.DTOs;

namespace BlueBerry24.Services.AuthAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponseDto> Register(RegisterRequestDto requestDto);
        Task<LoginResponseDto> Login(LoginRequestDto requestDto);
    }
}
