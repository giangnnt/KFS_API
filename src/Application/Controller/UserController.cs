using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.UserDtos;
using KFS.src.Application.Middleware;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;
using Microsoft.AspNetCore.Mvc;

namespace KFS.src.Application.Controller
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Protected]
        [HttpGet("admin/query")]
        public async Task<IActionResult> GetUsersAdmin(UserQuery userQuery)
        {
            var users = await _userService.GetUsersAdmin(userQuery);
            return Ok(users);
        }
        [Protected]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserById(id);
            return Ok(user);
        }
        [Protected]
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCreate userCreate)
        {
            var user = await _userService.CreateUser(userCreate);
            return Ok(user);
        }
        [Protected]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto, [FromQuery] string token)
        {
            var user = await _userService.ChangePassword(changePasswordDto, token);
            return Ok(user);
        }
        [Protected]
        [HttpPost("get-by-email")]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            var result = await _userService.GetUserByEmail(email);
            return Ok(result);
        }
        [Protected]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateUserStatus(Guid id, [FromQuery] bool IsActive)
        {
            var result = await _userService.UpdateUserStatus(id, IsActive);
            return Ok(result);
        }
    }
}