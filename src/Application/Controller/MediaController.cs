using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using KFS.src.Application.Constant;
using KFS.src.Application.Dto.MediaDtos;
using KFS.src.Application.Middleware;
using KFS.src.Domain.IService;
using Microsoft.AspNetCore.Mvc;

namespace KFS.src.Application.Controller
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/media")]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;
        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_MEDIA)]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadMedia([FromForm] IFormFile file, [FromForm] string type)
        {
            try
            {
                var result = await _mediaService.UploadMedia(file, type);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_MEDIA)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedia(Guid mediaId)
        {
            try
            {
                var result = await _mediaService.DeleteMedia(mediaId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_MEDIA, PermissionSlug.VIEW_MEDIA)]
        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetMediaByProductId(Guid productId)
        {
            try
            {
                var result = await _mediaService.GetMediaByProductId(productId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_MEDIA)]
        [HttpPut("product/{id}")]
        public async Task<IActionResult> UpdateMediaProduct(Guid productId, [FromBody] List<Guid> mediaIds)
        {
            try
            {
                var result = await _mediaService.UpdateMediaProduct(productId, mediaIds);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_MEDIA)]
        [HttpPost]
        public async Task<IActionResult> CreateMedia([FromBody] MediaCreate mediaCreate)
        {
            try
            {
                var result = await _mediaService.CreateMedia(mediaCreate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}