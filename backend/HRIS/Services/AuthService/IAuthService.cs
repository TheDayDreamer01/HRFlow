﻿using HRIS.Dtos.AuthDto;

namespace HRIS.Services.AuthService
{
    public interface IAuthService
    {
        Task<string> RegisterUser(RegisterUserDto request);
        Task<string> LoginUser(LoginUserDto request);
        Task<string> ForgotPassword(ForgotPasswordDto request);
        Task<string> VerifyPassword(OTPDto request);
    }
}
