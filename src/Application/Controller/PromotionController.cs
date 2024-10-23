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
        [HttpGet]
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
        [HttpPost]
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
        [HttpPut("{id}")]
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
        [HttpDelete("{id}")]
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
        [HttpPut("{id}/start")]
        public async Task<IActionResult> SetPromotionState(Guid id)
        {
            try
            {
                var result = await _promotionService.StartPromotion(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpPut("{id}/products")]
        public async Task<IActionResult> ListProductToPromotion(Guid promotionId, PromotionAddRemoveItemDto productId)
        {
            try
            {
                var result = await _promotionService.UpdateProductToPromotion(promotionId, productId.listId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpPut("{id}/batches")]
        public async Task<IActionResult> ListBatchToPromotion(Guid promotionId, PromotionAddRemoveItemDto batchId)
        {
            try
            {
                var result = await _promotionService.UpdateBatchToPromotion(promotionId, batchId.listId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpPut("{id}/categories")]
        public async Task<IActionResult> ListCategoryToPromotion(Guid promotionId, PromotionAddRemoveItemDto categoryId)
        {
            try
            {
                var result = await _promotionService.UpdateCategoryToPromotion(promotionId, categoryId.listId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpPut("{id}/end")]
        public async Task<IActionResult> EndPromotion(Guid promotionId)
        {
            try
            {
                var result = await _promotionService.EndPromotion(promotionId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}