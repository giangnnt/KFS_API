using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Constant;
using KFS.src.Application.Dto.CredentialDtos;
using KFS.src.Application.Middleware;
using KFS.src.Domain.IService;
using Microsoft.AspNetCore.Mvc;

namespace KFS.src.Application.Controller
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/credential")]
    public class CredentialController : ControllerBase
    {
        private readonly ICredentialService _credentialService;

        public CredentialController(ICredentialService credentialService)
        {
            _credentialService = credentialService;
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_CREDENTIAL, PermissionSlug.VIEW_CREDENTIAL)]
        [HttpGet]
        public async Task<IActionResult> GetCredentials()
        {
            try
            {
                var result = await _credentialService.GetCredentials();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_CREDENTIAL, PermissionSlug.VIEW_CREDENTIAL)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCredentialById(Guid id)
        {
            try
            {
                var result = await _credentialService.GetCredentialById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_CREDENTIAL, PermissionSlug.VIEW_CREDENTIAL)]
        [HttpPost]
        public async Task<IActionResult> CreateCredential(credentialCreate req)
        {
            try
            {
                var result = await _credentialService.CreateCredential(req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_CREDENTIAL)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCredential(CredentialUpdate req, Guid id)
        {
            try
            {
                var result = await _credentialService.UpdateCredential(req, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_CREDENTIAL)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCredential(Guid id)
        {
            try
            {
                var result = await _credentialService.DeleteCredential(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_CREDENTIAL, PermissionSlug.VIEW_CREDENTIAL)]
        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetCredentialsByProductId(Guid id)
        {
            try
            {
                var result = await _credentialService.GetCredentialsByProductId(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}