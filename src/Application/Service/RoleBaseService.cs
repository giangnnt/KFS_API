using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Application.Dto.RoleDtos;
using KFS.src.Application.Dto.UserDtos;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;

namespace KFS.src.Application.Service
{
    public class RoleBaseService : IRoleBaseService
    {
        private readonly IRoleBaseRepository _roleBaseRepository;
        private readonly IMapper _mapper;
        public RoleBaseService(IRoleBaseRepository roleBaseRepository, IMapper mapper)
        {
            _roleBaseRepository = roleBaseRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDto> CreateRole(RoleCreate role)
        {
            var response = new ResponseDto();
            try
            {
                var mappedRole = _mapper.Map<Role>(role);
                var result = await _roleBaseRepository.CreateRole(mappedRole);
                if (result)
                {
                    response.StatusCode = 201;
                    response.Message = "Role created successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Role creation failed";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception e)
            {
                response.StatusCode = 500;
                response.Message = e.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> DeleteRole(int roleId)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _roleBaseRepository.DeleteRole(roleId);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Role deleted successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Role deletion failed";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception e)
            {
                response.StatusCode = 500;
                response.Message = e.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> GetAllRoles()
        {
            var response = new ResponseDto();
            try
            {
                var roles = await _roleBaseRepository.GetAllRoles();
                var mappedRole = _mapper.Map<IEnumerable<RoleDto>>(await _roleBaseRepository.GetAllRoles());
                if (mappedRole != null && mappedRole.Count() > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Roles fetched successfully";
                    response.Result = new ResultDto
                    {
                        Data = mappedRole
                    };
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Roles not found";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception e)
            {
                response.StatusCode = 500;
                response.Message = e.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> GetPermissionRoleSlugs(int roleId)
        {
            var response = new ResponseDto();
            try
            {
                var permissionSlugs = await _roleBaseRepository.GetPermissionRoleSlugs(roleId);
                if (permissionSlugs != null && permissionSlugs.Count() > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Permission slugs fetched successfully";
                    response.Result = new ResultDto
                    {
                        Data = permissionSlugs
                    };
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Permission slugs not found";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception e)
            {
                response.StatusCode = 500;
                response.Message = e.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> GetRoleById(int roleId)
        {
            var response = new ResponseDto();
            try
            {
                var role = await _roleBaseRepository.GetRoleById(roleId);
                var mappedRole = _mapper.Map<RoleDto>(role);
                if (mappedRole != null)
                {
                    response.StatusCode = 200;
                    response.Message = "Role fetched successfully";
                    response.Result = new ResultDto
                    {
                        Data = mappedRole
                    };
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Role not found";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception e)
            {
                response.StatusCode = 500;
                response.Message = e.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> GetUsersByRole(int roleId)
        {
            var response = new ResponseDto();
            try
            {
                var users = await _roleBaseRepository.GetUsersByRole(roleId);
                var mappedUsers = _mapper.Map<IEnumerable<UserDto>>(users);
                if (mappedUsers != null && mappedUsers.Count() > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Users fetched successfully";
                    response.Result = new ResultDto
                    {
                        Data = mappedUsers
                    };
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Users not found";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception e)
            {
                response.StatusCode = 500;
                response.Message = e.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> UpdatePermissionRole(int roleId, List<string> permissionSlug)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _roleBaseRepository.UpdatePermissionRole(roleId, permissionSlug);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Permission updated successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Permission update failed";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception e)
            {
                response.StatusCode = 500;
                response.Message = e.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> UpdateRole(int id, RoleUpdate role)
        {
            var response = new ResponseDto();
            try
            {
                var roleToUpdate = await _roleBaseRepository.GetRoleById(id);
                var mappedRole = _mapper.Map(role, roleToUpdate);
                var result = await _roleBaseRepository.UpdateRole(mappedRole);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Role updated successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Role update failed";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception e)
            {
                response.StatusCode = 500;
                response.Message = e.Message;
                response.IsSuccess = false;
                return response;
            }
        }
    }
}