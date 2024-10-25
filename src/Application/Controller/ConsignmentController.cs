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
        [HttpPost("online")]
        public async Task<IActionResult> CreateConsignmentOnline([FromBody] ConsignmentCreateByOrderItem req)
        {
            var response = await _consignmentService.CreateConsignmentOnline(req);
            return Ok(response);
        }
        [Protected]
        [HttpPost("offline")]
        public async Task<IActionResult> CreateConsignment([FromBody] ConsignmentCreate req)
        {
            var response = await _consignmentService.CreateConsignment(req);
            return Ok(response);
        }
        [Protected]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConsignment(Guid id)
        {
            var response = await _consignmentService.UpdateConsignment(id);
            return Ok(response);
        }
        [Protected]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsignment(Guid id)
        {
            var response = await _consignmentService.DeleteConsignment(id);
            return Ok(response);
        }
        [Protected]
        [HttpPost("admin/query")]
        public async Task<IActionResult> GetConsignmentsAdmin(ConsignmentQuery consignmentQuery)
        {
            var response = await _consignmentService.GetConsignmentsAdmin(consignmentQuery);
            return Ok(response);
        }
        [Protected]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetConsignmentById(Guid id)
        {
            var response = await _consignmentService.GetConsignmentById(id);
            return Ok(response);
        }
        [Protected]
        [HttpPut("{id}/evaluate")]
        public async Task<IActionResult> EvaluateConsignment(bool isApproved, Guid id)
        {
            var response = await _consignmentService.EvaluateConsignment(isApproved, id);
            return Ok(response);
        }
        [Protected]
        [HttpPut("{id}/pay")]
        public async Task<IActionResult> PayForConsignment(Guid id)
        {
            var response = await _consignmentService.PayForConsignment(id);
            return Ok(response);
        }
        [HttpGet("vnpayment-return")]
        public async Task<IActionResult> GetResponsePaymentUrl()
        {
            var response = await _consignmentService.GetResponsePaymentUrl();
            return Ok(response);
        }
        [Protected]
        [HttpPost("user/{id}/query")]
        public async Task<IActionResult> GetConsignmentByUserId(ConsignmentQuery consignmentQuery,Guid userId)
        {
            var response = await _consignmentService.GetConsignmentByUserId(consignmentQuery, userId);
            return Ok(response);
        }
        [Protected]
        [HttpPost("own/query")]
        public async Task<IActionResult> GetOwnConsignment(ConsignmentQuery consignmentQuery)
        {
            var response = await _consignmentService.GetOwnConsignment(consignmentQuery);
            return Ok(response);
        }
    }
}