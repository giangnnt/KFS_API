using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.ShipmentDtos;
using KFS.src.Application.Enum;
using KFS.src.Application.Middleware;
using KFS.src.Domain.IService;
using Microsoft.AspNetCore.Mvc;

namespace KFS.src.Application.Controller
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/shipment")]
    public class ShipmentController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;
        public ShipmentController(IShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }
        [Protected]
        [HttpPost]
        public async Task<IActionResult> CreateShipment(Guid orderId)
        {
            try
            {
                var result = await _shipmentService.CreateShipment(orderId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateShipment(UpdateDto updateDto, Guid id)
        {
            try
            {
                var result = await _shipmentService.UpdateShipment(updateDto, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipment(Guid id)
        {
            try
            {
                var result = await _shipmentService.DeleteShipment(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpPost("query")]
        public async Task<IActionResult> GetShipments(ShipmentQuery shipmentQuery)
        {
            try
            {
                var result = await _shipmentService.GetShipments(shipmentQuery);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShipmentById(Guid id)
        {
            try
            {
                var result = await _shipmentService.GetShipmentById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpPut("{id}/delivered/is-success")]
        public async Task<IActionResult> ShipmentDelivered(Guid id, [FromQuery] bool IsSuccess)
        {
            try
            {
                var result = await _shipmentService.ShipmentDelivered(id, IsSuccess);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpPut("{id}/completed/is-success")]
        public async Task<IActionResult> ShipmentCompleted(Guid id, [FromQuery] bool IsSuccess)
        {
            try
            {
                var result = await _shipmentService.ShipmentCompleted(id, IsSuccess);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}