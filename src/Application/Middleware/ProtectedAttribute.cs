using KFS.src.Application.Core.Jwt;
using KFS.src.Application.Dto.ResponseDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KFS.src.Application.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ProtectedAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IJwtService _jwtService;

        public ProtectedAttribute()
        {
            _jwtService = new JwtService();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                // Trả về sớm nếu token không có hoặc không đúng định dạng
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new JsonResult(new ResponseDto
                {
                    StatusCode = 401,
                    Message = "Unauthorized",
                    IsSuccess = false
                });
                return;
            }

            var token = authorizationHeader.Replace("Bearer ", "");

            try
            {
                var payload = _jwtService.ValidateToken(token);
                context.HttpContext.Items["payload"] = payload;
            }
            catch (Exception ex)
            {
                // Token không hợp lệ
                context.Result = new JsonResult(new ResponseDto
                {
                    StatusCode = 401,
                    Message = ex.Message,
                    IsSuccess = false
                });
                return;
            }
        }

    }
}