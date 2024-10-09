using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.ConsignmentDtos;
using KFS.src.Application.Middleware;
using KFS.src.Domain.IService;
using Microsoft.AspNetCore.Mvc;

namespace KFS.src.Application.Controller
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/consignment")]
    public class ConsignmentController : ControllerBase
    {
        private readonly IConsignmentService _consignmentService;
        public ConsignmentController(IConsignmentService consignmentService)
        {
            _consignmentService = consignmentService;
        }
        [Protected]
        [HttpPost("create-consignment-online")]
        public async Task<IActionResult> CreateConsignmentOnline([FromBody] ConsignmentCreateByOrderItem req)
        {
            var response = await _consignmentService.CreateConsignmentOnline(req);
            return Ok(response);
        }
        [Protected]
        [HttpPost("create-consignment")]
        public async Task<IActionResult> CreateConsignment([FromBody] ConsignmentCreate req)
        {
            var response = await _consignmentService.CreateConsignment(req);
            return Ok(response);
        }
        [Protected]
        [HttpPut("update-consignment/{id}")]
        public async Task<IActionResult> UpdateConsignment(Guid id)
        {
            var response = await _consignmentService.UpdateConsignment(id);
            return Ok(response);
        }
        [Protected]
        [HttpDelete("delete-consignment/{id}")]
        public async Task<IActionResult> DeleteConsignment(Guid id)
        {
            var response = await _consignmentService.DeleteConsignment(id);
            return Ok(response);
        }
        [Protected]
        [HttpGet("get-consignments")]
        public async Task<IActionResult> GetConsignments()
        {
            var response = await _consignmentService.GetConsignments();
            return Ok(response);
        }
        [Protected]
        [HttpGet("get-consignment-by-id/{id}")]
        public async Task<IActionResult> GetConsignmentById(Guid id)
        {
            var response = await _consignmentService.GetConsignmentById(id);
            return Ok(response);
        }
        [Protected]
        [HttpPut("evaluate-consignment/{id}")]
        public async Task<IActionResult> EvaluateConsignment(bool isApproved, Guid id)
        {
            var response = await _consignmentService.EvaluateConsignment(isApproved, id);
            return Ok(response);
        }
    }
}