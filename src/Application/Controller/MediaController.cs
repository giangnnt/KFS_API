using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using KFS.src.Application.Dto.MediaDtos;
using KFS.src.Application.Dto.ProductDtos;
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

            public MediaController(IMediaService merService)
            {
                _mediaService = merService;
            }
            [HttpGet("all")]
            public async Task<IActionResult> GetMedia()
            {
                try
                {
                    var result = await _mediaService.GetMedias();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            [HttpGet("{id}")]
            public async Task<IActionResult> GetMediaById(Guid id)
            {
                try
                {
                    var result = await _mediaService.GetMediaById(id);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            [Protected]
            [HttpPost("create")]
            public async Task<IActionResult> CreateProduct([FromBody] MediaCreate req)
            {
                try
                {
                    var result = await _mediaService.Create(req);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            [Protected]
            [HttpPut("update/{id}")]
            public async Task<IActionResult> UpdateProduct([FromBody] MediaUpdate req, Guid id)
            {
                try
                {
                    var result = await _mediaService.Update(id, req);
                    return Ok(result);
                }
                catch (Exception ex)
        {
            _mediaService = mediaService;
                    return BadRequest(ex.Message);
                }
        }
        [Protected]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadMedia([FromForm] IFormFile file, [FromForm] string type)
            [HttpDelete("delete/{id}")]
            public async Task<IActionResult> DeleteMedia(Guid id)
        {
            try
            {
                var result = await _mediaService.UploadMedia(file, type);
                    var result = await _mediaService.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
          
    }
}
    

