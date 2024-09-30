using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BCrypt.Net;
using KFS.src.Application.Constant;
using KFS.src.Application.Core.Crypto;
using KFS.src.Application.Core.Jwt;
using KFS.src.Application.Dto.AuthDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Application.Dto.Session;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;
using KFS.src.Infrastucture.Cache;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace KFS.src.Application.Service
{
    public class AuthService : IAuthService
    {
        private readonly IRoleBaseRepository _roleBaseRepository;
        private readonly IUserRepository _userRepository;
        private readonly Crypto _crypto;
        private readonly JwtService _jwtService;
        private readonly ICacheService _cacheService;
        public AuthService(IRoleBaseRepository roleBaseRepository, IUserRepository userRepository, Crypto crypto, JwtService jwtService, ICacheService cacheService)
        {
            _roleBaseRepository = roleBaseRepository;
            _userRepository = userRepository;
            _crypto = crypto;
            _jwtService = jwtService;
            _cacheService = cacheService;
        }

        public async Task<ResponseDto> Login(LoginDto loginDto)
        {
            var response = new ResponseDto();
            try
            {
                // Check if user exists
                var user = await _userRepository.GetUserByEmail(loginDto.Email);
                if (user == null)
                {
                    response.StatusCode = 404;
                    response.Message = "User not found";
                    response.IsSuccess = false;
                }
                // Verify password
                if (!_crypto.VerifyPassword(loginDto.Password, user!.Password))
                {
                    response.StatusCode = 401;
                    response.Message = "Invalid password";
                    response.IsSuccess = false;
                }
                Guid sessionId = Guid.NewGuid();
                var accessToken = _jwtService.GenerateToken(user.Id, sessionId, user.RoleId, JwtConst.ACCESS_TOKEN_EXP);
                var refreshToken = GenerateRefreshTk();

                // create redis refresh token key
                var redisRfTkKey = $"rfTk:{user.Id}:{refreshToken}";
                await _cacheService.Set(redisRfTkKey, new RedisSession
                {
                    UserId = user.Id,
                    SessionId = sessionId
                }, TimeSpan.FromSeconds(JwtConst.REFRESH_TOKEN_EXP));

                // create a session id key
                var sessionKey = $"session:{user.Id}:{sessionId}";
                await _cacheService.Set(sessionKey, new RedisSession
                {
                    UserId = user.Id,
                    SessionId = sessionId,
                    Refresh = refreshToken
                }, TimeSpan.FromSeconds(JwtConst.REFRESH_TOKEN_EXP));
                TokenResp tokenResp = new TokenResp
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    AccessTokenExp = DateTimeOffset.UtcNow.AddSeconds(JwtConst.ACCESS_TOKEN_EXP).ToUnixTimeSeconds(),
                    RefreshTokenExp = DateTimeOffset.UtcNow.AddSeconds(JwtConst.REFRESH_TOKEN_EXP).ToUnixTimeSeconds()
                };
                response.StatusCode = 200;
                response.Message = "Login successful";
                response.Result = new ResultDto
                {
                    Data = tokenResp
                };
                return response;
            }
            catch 
            {
                throw;
            }
        }

        public Task<ResponseDto> RefreshToken(string token)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> Register(RegisterDto registerDto)
        {
            throw new NotImplementedException();
        }
        private static string GenerateRefreshTk()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}