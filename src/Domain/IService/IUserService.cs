using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Application.Dto.UserDtos;

namespace KFS.src.Domain.IService
{
    public interface IUserService
    {
        Task<ResponseDto> ChangePassword(ChangePasswordDto req, string token);
        Task<ResponseDto> GetUsersAdmin(UserQuery userQuery);
        Task<ResponseDto> UpdateUserStatus(Guid id, bool IsActive);
        Task<ResponseDto> GetUserByEmail(string email);
        Task<ResponseDto> GetUserById(Guid id);
        Task<ResponseDto> CreateUser(UserCreate req);
        Task<ResponseDto> UpdateUser(UserUpdate req);
    }
}