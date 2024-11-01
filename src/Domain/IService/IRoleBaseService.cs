using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Constant;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Application.Dto.RoleDtos;

namespace KFS.src.Domain.IService
{
    public interface IRoleBaseService
    {
        public Task<ResponseDto> GetPermissionRoleSlugs(int roleId);
        public Task<ResponseDto> GetUsersByRole(int roleId);
        public Task<ResponseDto> UpdatePermissionRole(int roleId, List<string> permissionSlug);
        public Task<ResponseDto> CreateRole(RoleCreate role);
        public Task<ResponseDto> DeleteRole(int roleId);
        public Task<ResponseDto> GetAllRoles();
        public Task<ResponseDto> GetRoleById(int roleId);
        public Task<ResponseDto> UpdateRole(RoleUpdate role);
    }
}