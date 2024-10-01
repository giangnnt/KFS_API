using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.AuthDtos;
using KFS.src.Application.Dto.ResponseDtos;

namespace KFS.src.Domain.IService
{
    public interface IAuthService
    {
        Task<ResponseDto> Login(LoginDto loginDto);
        Task<ResponseDto> Register(RegisterDto registerDto);
        Task<ResponseDto> RefreshToken(string token);
        Task<ResponseDto> Logout(string token);
    }
}