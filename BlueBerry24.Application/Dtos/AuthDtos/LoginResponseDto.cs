﻿namespace BlueBerry24.Application.Dtos.AuthDtos
{
    public class LoginResponseDto
    {
        public ApplicationUserDto User { get; set; }
        public string Token { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
