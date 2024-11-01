using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Application.Dto.RoleDtos;
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

        public Task<ResponseDto> DeleteRole(int roleId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> GetPermissionRoleSlugs(int roleId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> GetRoleById(int roleId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> GetUsersByRole(int roleId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> UpdatePermissionRole(int roleId, List<string> permissionSlug)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> UpdateRole(RoleUpdate role)
        {
            throw new NotImplementedException();
        }
    }
}