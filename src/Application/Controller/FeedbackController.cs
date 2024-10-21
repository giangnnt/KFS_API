using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using KFS.src.Application.Dto.FeedbackDtos;
using KFS.src.Application.Middleware;
using KFS.src.Domain.IService;
using Microsoft.AspNetCore.Mvc;

namespace KFS.src.Application.Controller
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/feedback")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _feedbackService.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeedbackById(Guid id)
        {
            try
            {
                var result = await _feedbackService.GetFeedbackById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Protected]
        [HttpPost("create")]
        public async Task<IActionResult> CreateFeedback([FromBody] FeedbackCreate req)
        {
            try
            {
                var result = await _feedbackService.Create(req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Protected]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateFeedback([FromBody] FeedbackUpdate req, Guid id)
        {
            try
            {
                var result = await _feedbackService.Update(id, req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Protected]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteFeedback(Guid id)
        {
            try
            {
                var result = await _feedbackService.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}