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
    [Route("api/consignmets")]
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
    }
}