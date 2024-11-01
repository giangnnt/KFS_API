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
    }
}