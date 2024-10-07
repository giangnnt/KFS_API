using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        [Protected]
        [HttpGet("all")]
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
        [Protected]
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
        [HttpPost("create")]
        public async Task<IActionResult> CreateBatch(BatchCreate req, Guid productId)
        {
            try
            {
                var result = await _batchService.CreateBatch(req, productId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpPut("update/{id}")]
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
        [HttpDelete("delete/{id}")]
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