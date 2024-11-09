using KFS.src.Application.Constant;
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
        [HttpPost("query")]
        public async Task<IActionResult> GetFeedbacks(FeedbackQuery feedbackQuery)
        {
            try
            {
                var result = await _feedbackService.GetFeedbacks(feedbackQuery);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_FEEDBACK, PermissionSlug.MANAGE_OWN_FEEDBACK)]
        [HttpPost("product/{id}")]
        public async Task<IActionResult> CreateFeedback(Guid id, FeedbackCreate feedbackCreate)
        {
            try
            {
                var result = await _feedbackService.CreateFeedback(id, feedbackCreate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_FEEDBACK, PermissionSlug.MANAGE_OWN_FEEDBACK)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeedback(Guid id, FeedbackUpdate feedbackUpdate)
        {
            try
            {
                var result = await _feedbackService.UpdateFeedback(id, feedbackUpdate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_FEEDBACK, PermissionSlug.MANAGE_OWN_FEEDBACK)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(Guid id)
        {
            try
            {
                var result = await _feedbackService.DeleteFeedback(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetAverageRating(Guid id)
        {
            try
            {
                var result = await _feedbackService.GetAverageRating(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetFeedbackByUserId(Guid id)
        {
            try
            {
                var result = await _feedbackService.GetFeedbackByUserId(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}