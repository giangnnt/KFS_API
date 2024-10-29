using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KFS.src.Application.Constant;
using KFS.src.Application.Core.Crypto;
using KFS.src.Application.Core.Jwt;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Application.Dto.Session;
using KFS.src.Application.Dto.UserDtos;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;
using KFS.src.Infrastucture.Cache;

namespace KFS.src.Application.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICrypto _crypto;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, ICrypto crypto, ICacheService cacheService, IMapper mapper)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _crypto = crypto;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<ResponseDto> ChangePassword(ChangePasswordDto req, string token)
        {
            var response = new ResponseDto();
            try
            {
                // get http context
                var httpContext = _httpContextAccessor.HttpContext;
                //  check http context
                if (httpContext == null)
                {
                    response.StatusCode = 500;
                    response.Message = "Internal Server Error";
                    response.IsSuccess = false;
                    return response;
                }
                // get payload from http context
                var payload = httpContext.Items["payload"] as Payload;
                // check payload
                if (payload == null)
                {
                    response.StatusCode = 401;
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return response;
                }
                // get and remove rfTk
                var redisRfTkKey = $"rfTk:{token}";
                // check rfTk
                var redisrfTk = await _cacheService.Get<RedisSession>(redisRfTkKey);
                if (redisrfTk == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Token not found";
                    response.IsSuccess = false;
                    return response;
                }
                // get user by id
                var user = await _userRepository.GetUserById(payload.UserId);
                // check user
                if (user == null)
                {
                    response.StatusCode = 404;
                    response.Message = "User not found";
                    response.IsSuccess = false;
                    return response;
                }
                // check old password
                if (!_crypto.VerifyPassword(req.OldPassword, user.Password))
                {
                    response.StatusCode = 400;
                    response.Message = "Old password is incorrect";
                    response.IsSuccess = false;
                    return response;
                }
                // hash new password
                user.Password = _crypto.HashPassword(req.NewPassword);
                // update user
                var result = await _userRepository.UpdateUser(user);
                await _cacheService.Remove(redisRfTkKey);
                await _cacheService.ForceLogout(user.Id);
                // check result
                if (!result)
                {
                    response.StatusCode = 500;
                    response.Message = "Internal Server Error";
                    response.IsSuccess = false;
                    return response;
                }
                else
                {
                    response.StatusCode = 200;
                    response.Message = "Change password successfully";
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> CreateUser(UserCreate req)
        {
            var response = new ResponseDto();
            try
            {
                var mappedUser = _mapper.Map<User>(req);
                mappedUser.Password = _crypto.HashPassword("123456");
                mappedUser.IsActive = false;
                // create user
                var result = await _userRepository.CreateUser(mappedUser);
                // check result
                if (!result)
                {
                    response.StatusCode = 500;
                    response.Message = "Internal Server Error";
                    response.IsSuccess = false;
                    return response;
                }
                else
                {
                    response.StatusCode = 201;
                    response.Message = "Create user successfully";
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> GetUserByEmail(string email)
        {
            var response = new ResponseDto();
            try
            {
                var user = await _userRepository.GetUserByEmail(email);
                var mappedUser = _mapper.Map<UserDto>(user);
                if (user == null)
                {
                    response.StatusCode = 404;
                    response.Message = "User not found";
                    response.IsSuccess = false;
                    return response;
                }
                else
                {
                    response.StatusCode = 200;
                    response.Message = "Get user successfully";
                    response.Result = new ResultDto
                    {
                        Data = mappedUser
                    };
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> GetUserById(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var user = await _userRepository.GetUserById(id);
                var mappedUser = _mapper.Map<UserDto>(user);
                if (user == null)
                {
                    response.StatusCode = 404;
                    response.Message = "User not found";
                    response.IsSuccess = false;
                    return response;
                }
                else
                {
                    response.StatusCode = 200;
                    response.Message = "Get user successfully";
                    response.Result = new ResultDto
                    {
                        Data = mappedUser
                    };
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> GetUsersAdmin(UserQuery userQuery)
        {
            var response = new ResponseDto();
            try
            {
                var user = await _userRepository.GetUsersAdmin(userQuery);
                var mappedUsers = _mapper.Map<List<UserDto>>(user.List);
                if (user == null || user.List.Count() == 0)
                {
                    response.StatusCode = 404;
                    response.Message = "User not found";
                    response.IsSuccess = false;
                    return response;
                }
                else
                {
                    response.StatusCode = 200;
                    response.Message = "Get user successfully";
                    response.Result = new ResultDto
                    {
                        Data = mappedUsers
                    };
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public Task<ResponseDto> UpdateUser(UserUpdate req)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto> UpdateUserStatus(Guid id, bool IsActive)
        {
            var response = new ResponseDto();
            try
            {
                var user = await _userRepository.GetUserById(id);
                if (user == null)
                {
                    response.StatusCode = 404;
                    response.Message = "User not found";
                    response.IsSuccess = false;
                    return response;
                }
                user.IsActive = IsActive;
                var result = await _userRepository.UpdateUser(user);
                if (!result)
                {
                    response.StatusCode = 500;
                    response.Message = "Internal Server Error";
                    response.IsSuccess = false;
                    return response;
                }
                else
                {
                    response.StatusCode = 200;
                    response.Message = "Update user status successfully";
                    response.IsSuccess = true;
                    return response;
                }
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