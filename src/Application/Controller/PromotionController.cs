using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.PromotionDtos;
using KFS.src.Application.Middleware;
using KFS.src.Domain.IService;
using Microsoft.AspNetCore.Mvc;

namespace KFS.src.Application.Controller
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/promotion")]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService _promotionService;
        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }
        [Protected]
        [HttpGet("all")]
        public async Task<IActionResult> GetPromotions()
        {
            try
            {
                var result = await _promotionService.GetPromotions();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPromotionById(Guid id)
        {
            try
            {
                var result = await _promotionService.GetPromotionById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpPost("create")]
        public async Task<IActionResult> CreatePromotion(PromotionCreate req)
        {
            try
            {
                var result = await _promotionService.CreatePromotion(req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdatePromotion(PromotionUpdate req, Guid id)
        {
            try
            {
                var result = await _promotionService.UpdatePromotion(req, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePromotion(Guid id)
        {
            try
            {
                var result = await _promotionService.DeletePromotion(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpPut("isActive/{id}")]
        public async Task<IActionResult> SetPromotionState(Guid id, bool IsActive)
        {
            try
            {
                var result = await _promotionService.SetPromotionIsActive(id, IsActive);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpPut("list-product/{promotionId}")]
        public async Task<IActionResult> ListProductToPromotion(Guid promotionId, List<Guid> productId)
        {
            try
            {
                var result = await _promotionService.ListProductToPromotion(promotionId, productId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpPut("list-batch/{promotionId}")]
        public async Task<IActionResult> ListBatchToPromotion(Guid promotionId, List<Guid> batchId)
        {
            try
            {
                var result = await _promotionService.ListBatchToPromotion(promotionId, batchId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpPut("list-category/{promotionId}")]
        public async Task<IActionResult> ListCategoryToPromotion(Guid promotionId, List<Guid> categoryId)
        {
            try
            {
                var result = await _promotionService.ListCategoryToPromotion(promotionId, categoryId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}