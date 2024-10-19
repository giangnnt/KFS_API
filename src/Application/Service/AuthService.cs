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
using KFS.src.Domain.Entities;
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
        private readonly ICrypro _crypto;
        private readonly IJwtService _jwtService;
        private readonly ICacheService _cacheService;
        public AuthService(IRoleBaseRepository roleBaseRepository, IUserRepository userRepository, ICrypro crypro, IJwtService jwtService, ICacheService cacheService)
        {
            _roleBaseRepository = roleBaseRepository;
            _userRepository = userRepository;
            _crypto = crypro;
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
                    return response;
                }
                // Verify password
                if (!_crypto.VerifyPassword(loginDto.Password, user!.Password))
                {
                    response.StatusCode = 401;
                    response.Message = "Invalid password";
                    response.IsSuccess = false;
                    return response;
                }
                Guid sessionId = Guid.NewGuid();
                var accessToken = _jwtService.GenerateToken(user.Id, sessionId, user.RoleId, JwtConst.ACCESS_TOKEN_EXP);
                var refreshToken = GenerateRefreshTk();

                // create redis refresh token key
                var redisRfTkKey = $"rfTk:{refreshToken}";
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
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> RefreshToken(string token)
        {
            var response = new ResponseDto();
            try
            {
                // check if token exists
                var redisRfTkKey = $"rfTk:{token}";
                var redisrfTk = await _cacheService.Get<RedisSession>(redisRfTkKey);
                if (redisrfTk == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Token not found";
                    response.IsSuccess = false;
                    return response;
                }
                var ssId = redisrfTk.SessionId;
                var userId = redisrfTk.UserId;
                var user = await _userRepository.GetUserById(redisrfTk.UserId);
                if (user == null)
                {
                    response.StatusCode = 404;
                    response.Message = "User not found";
                    response.IsSuccess = false;
                    return response;
                }
                var redisSessionKey = $"session:{userId}:{ssId}";
                var redisSession = await _cacheService.Get<RedisSession>(redisSessionKey);
                if (redisSession == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Session not found";
                    response.IsSuccess = false;
                    return response;
                }
                if (redisSession.Refresh != token)
                {
                    response.StatusCode = 401;
                    response.Message = "Invalid token";
                    response.IsSuccess = false;
                    return response;
                }
                var newAccessToken = _jwtService.GenerateToken(user.Id, ssId, user.RoleId, JwtConst.ACCESS_TOKEN_EXP);
                TokenResp tokenResp = new TokenResp
                {
                    AccessToken = newAccessToken,
                    RefreshToken = token,
                    AccessTokenExp = DateTimeOffset.UtcNow.AddSeconds(JwtConst.ACCESS_TOKEN_EXP).ToUnixTimeSeconds(),
                    RefreshTokenExp = -1
                };
                response.StatusCode = 200;
                response.Message = "Token refreshed";
                response.Result = new ResultDto
                {
                    Data = tokenResp
                };
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> Register(RegisterDto registerDto)
        {
            var response = new ResponseDto();
            try
            {
                // Check if user exists
                var user = await _userRepository.GetUserByEmail(registerDto.Email);
                if (user != null)
                {
                    response.StatusCode = 400;
                    response.Message = "User already exists";
                    response.IsSuccess = false;
                }
                else
                {
                    var hashedPassword = _crypto.HashPassword(registerDto.Password);
                    var newUser = new User
                    {
                        Email = registerDto.Email,
                        Password = hashedPassword,
                        FullName = registerDto.FullName,
                        Phone = registerDto.PhoneNumber,
                        RoleId = RoleConst.GetRoleId(RoleConst.CUSTOMER)
                    };
                    var result = await _userRepository.CreateUser(newUser);
                    if (result)
                    {
                        response.StatusCode = 201;
                        response.Message = "User created successfully";
                        response.IsSuccess = true;
                    }
                    else
                    {
                        response.StatusCode = 500;
                        response.Message = "Failed to create user";
                        response.IsSuccess = false;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }
        private static string GenerateRefreshTk()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<ResponseDto> Logout(string token)
        {
            var response = new ResponseDto();
            try
            {
                var redisRfTkKey = $"rfTk:{token}";
                var redisrfTk = await _cacheService.Get<RedisSession>(redisRfTkKey);
                if (redisrfTk == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Token not found";
                    response.IsSuccess = false;
                    return response;
                }
                var ssId = redisrfTk.SessionId;
                var userId = redisrfTk.UserId;
                var user = await _userRepository.GetUserById(redisrfTk.UserId);
                if (user == null)
                {
                    response.StatusCode = 404;
                    response.Message = "User not found";
                    response.IsSuccess = false;
                    return response;
                }
                var redisSessionKey = $"session:{userId}:{ssId}";
                var redisSession = await _cacheService.Get<RedisSession>(redisSessionKey);
                if (redisSession == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Session not found";
                    response.IsSuccess = false;
                    return response;
                }
                if (redisSession.Refresh != token)
                {
                    response.StatusCode = 401;
                    response.Message = "Invalid token";
                    response.IsSuccess = false;
                    return response;
                }
                // remove refresh token and session
                await _cacheService.Remove(redisRfTkKey);
                await _cacheService.Remove(redisSessionKey);

                response.StatusCode = 200;
                response.Message = "Logout successful";
                response.IsSuccess = true;
                return response;

            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }
    }
}