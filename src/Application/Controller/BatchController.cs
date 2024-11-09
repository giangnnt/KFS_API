using KFS.src.Application.Constant;
using KFS.src.Application.Dto.BatchDtos;
using KFS.src.Application.Middleware;
using KFS.src.Domain.IService;
using Microsoft.AspNetCore.Mvc;

namespace KFS.src.Application.Controller
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/batch")]
    public class BatchController : ControllerBase
    {
        private readonly IBatchService _batchService;
        public BatchController(IBatchService batchService)
        {
            _batchService = batchService;
        }
        [HttpGet]
        public async Task<IActionResult> GetBatches()
        {
            try
            {
                var result = await _batchService.GetBatches();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBatchById(Guid id)
        {
            try
            {
                var result = await _batchService.GetBatchById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_BATCH)]
        [HttpPost("product/{id}")]
        public async Task<IActionResult> CreateBatch(BatchCreate req, Guid id)
        {
            try
            {
                var result = await _batchService.CreateBatchFromProduct(req, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_BATCH)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBatch(BatchUpdate req, Guid id)
        {
            try
            {
                var result = await _batchService.UpdateBatch(req, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_BATCH)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBatch(Guid id)
        {
            try
            {
                var result = await _batchService.DeleteBatch(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}