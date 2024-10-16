using KFS.src.Application.Dto.MediaDtos;
using KFS.src.Application.Service;
using KFS.src.Domain.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace KFS.src.Application.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;
        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetMedias()
        {
            try
            {
                var result = await _mediaService.GetMedias();
               return Ok (result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }
        [HttpPost("create")]
        public async Task<IActionResult> create(MediaCreate media)
        {
            try
            {
                var result = await _mediaService.Create(media);
                return Ok(result);
            }
            catch
            {
                return BadRequest("error in proccessing");
            }

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _mediaService.GetMediaById(id);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] MediaUpdate media)
        {
            try
            {
                var result = await _mediaService.Update(id, media);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return StatusCode((int)result.StatusCode, result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> delete(Guid id)
{
    try
    {
        var result = await _mediaService.Delete(id);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        else
        {
            throw new Exception();
        }
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
    }
}
    }
}
