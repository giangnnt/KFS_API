using KFS.src.Application.Constant;
using KFS.src.Application.Dto.RoleDtos;
using KFS.src.Application.Middleware;
using KFS.src.Domain.IService;
using Microsoft.AspNetCore.Mvc;

namespace KFS.src.Application.Controller
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/role")]
    public class RoleBaseController : ControllerBase
    {
        private readonly IRoleBaseService _roleBaseService;
        public RoleBaseController(IRoleBaseService roleBaseService)
        {
            _roleBaseService = roleBaseService;
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_ROLE)]
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleCreate role)
        {
            try
            {
                var response = await _roleBaseService.CreateRole(role);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_ROLE)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var response = await _roleBaseService.DeleteRole(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_ROLE, PermissionSlug.VIEW_ROLE)]
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var response = await _roleBaseService.GetAllRoles();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_ROLE, PermissionSlug.VIEW_ROLE)]
        [HttpGet("{id}/permissions")]
        public async Task<IActionResult> GetPermissionRoleSlugs(int id)
        {
            try
            {
                var response = await _roleBaseService.GetPermissionRoleSlugs(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_ROLE, PermissionSlug.VIEW_ROLE)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            try
            {
                var response = await _roleBaseService.GetRoleById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_ROLE, PermissionSlug.VIEW_ROLE)]
        [HttpGet("{id}/users")]
        public async Task<IActionResult> GetUsersByRole(int id)
        {
            try
            {
                var response = await _roleBaseService.GetUsersByRole(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_ROLE)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole([FromBody] RoleUpdate role, int id)
        {
            try
            {
                var response = await _roleBaseService.UpdateRole(id, role);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_ROLE)]
        [HttpPut("{id}/permissions")]
        public async Task<IActionResult> UpdatePermissionRole(int id, [FromBody] List<string> permissionSlug)
        {
            try
            {
                var response = await _roleBaseService.UpdatePermissionRole(id, permissionSlug);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}