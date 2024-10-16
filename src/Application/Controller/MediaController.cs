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
        [HttpGet]
        public async Task<IActionResult> getall()
        {
            try
            {
                var result = await _mediaService.getallmedia();
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
        [HttpPost]
        public async Task<IActionResult> create(MediaCreate media)
        {
            try
            {
                var result = await _mediaService.create(media);
                return Ok(result);
            }
            catch
            {
                return BadRequest("error in proccessing");
            }

        }
        [HttpGet ("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _mediaService.getmediabyid(id);
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
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] MediaUpdate media)
        {
            try
            {
                var result = await _mediaService.update(id, media);
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
        [HttpDelete ("{id}")]
        public async Task<IActionResult> delete(Guid id)
{
    try
    {
        var result = await _mediaService.deletemedia(id);
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
