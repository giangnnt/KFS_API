using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Constant;
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
        [Permission(PermissionSlug.MANAGE_PROMOTION)]
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
        [Permission(PermissionSlug.MANAGE_PROMOTION)]
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
        [Permission(PermissionSlug.MANAGE_PROMOTION)]
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
        [Permission(PermissionSlug.MANAGE_PROMOTION)]
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
        [Permission(PermissionSlug.MANAGE_PROMOTION)]
        [HttpPut("{id}/products")]
        public async Task<IActionResult> ListProductToPromotion(Guid id, PromotionAddRemoveItemDto productId)
        {
            try
            {
                var result = await _promotionService.UpdateProductToPromotion(id, productId.listId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_PROMOTION)]
        [HttpPut("{id}/batches")]
        public async Task<IActionResult> ListBatchToPromotion(Guid id, PromotionAddRemoveItemDto batchId)
        {
            try
            {
                var result = await _promotionService.UpdateBatchToPromotion(id, batchId.listId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_PROMOTION)]
        [HttpPut("{id}/categories")]
        public async Task<IActionResult> ListCategoryToPromotion(Guid id, PromotionAddRemoveItemDto categoryId)
        {
            try
            {
                var result = await _promotionService.UpdateCategoryToPromotion(id, categoryId.listId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_PROMOTION)]
        [HttpPut("{id}/end")]
        public async Task<IActionResult> EndPromotion(Guid id)
        {
            try
            {
                var result = await _promotionService.EndPromotion(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}