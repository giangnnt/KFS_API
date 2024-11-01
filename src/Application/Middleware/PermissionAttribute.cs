using KFS.src.Application.Core.Jwt;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Domain.IRepository;
using KFS.src.Infrastucture.Cache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KFS.src.Application.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class PermissionAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _permission;

        public PermissionAttribute(params string[] permission)
        {
            _permission = permission;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            Task.Run(async () => await OnAuthorizationAsync(context)).Wait();
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var serviceProvider = context.HttpContext.RequestServices;
            var roleBaseRepository = serviceProvider.GetService<IRoleBaseRepository>();
            var cacheService = serviceProvider.GetService<ICacheService>();
            try
            {
                var isForbidden = true;
                var payload = context.HttpContext.Items["payload"] as Payload;
                if (payload == null)
                {
                    isForbidden = true;
                    context.Result = new JsonResult(new ResponseDto
                    {
                        StatusCode = 401,
                        Message = "Unauthorized",
                        IsSuccess = false
                    });
                    return;
                }
                if (roleBaseRepository == null || cacheService == null)
                {
                    context.Result = new JsonResult(new ResponseDto
                    {
                        StatusCode = 500,
                        Message = "Service not available",
                        IsSuccess = false
                    });
                    return;
                }
                else
                {
                    var redisey = $"roles:{payload.RoleId}:permissions";
                    var PermissionRoleCache = await cacheService.Get<List<string>>(redisey);
                    if (PermissionRoleCache == null)
                    {
                        var PermissionRoles = roleBaseRepository.GetPermissionRoleSlugs(payload.RoleId);
                        if (PermissionRoles != null)
                        {
                            await cacheService.Set(redisey, PermissionRoles);
                            foreach (var permission in _permission)
                            {
                                if (PermissionRoles.Contains(permission))
                                {
                                    isForbidden = false;
                                    context.HttpContext.Items["permission"] = _permission;
                                    break;
                                }
                            }
                        }
                    }
                    if (PermissionRoleCache != null)
                    {
                        foreach (var permission in _permission)
                        {
                            if (PermissionRoleCache.Contains(permission))
                            {
                                isForbidden = false;
                                context.HttpContext.Items["permission"] = _permission;
                                break;
                            }
                        }
                    }
                }
                if (isForbidden)
                {
                    context.Result = new JsonResult(new ResponseDto
                    {
                        StatusCode = 403,
                        Message = "Forbidden",
                        IsSuccess = false
                    });
                }

            }
            catch (Exception)
            {
                context.Result = new JsonResult(new ResponseDto
                {
                    StatusCode = 500,
                    Message = "Internal Server Error",
                    IsSuccess = false
                });
            }
        }
    }
}