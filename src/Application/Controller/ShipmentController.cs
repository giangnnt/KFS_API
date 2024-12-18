using KFS.src.Application.Constant;
using KFS.src.Application.Dto.ShipmentDtos;
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
        [Permission(PermissionSlug.MANAGE_SHIPMENT)]
        [HttpPost]
        public async Task<IActionResult> CreateShipment(Guid orderId, Guid shipperId)
        {
            try
            {
                var result = await _shipmentService.CreateShipment(orderId, shipperId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_SHIPMENT)]
        [HttpPut("{id}")]
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
        [Permission(PermissionSlug.MANAGE_SHIPMENT)]
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
        [Permission(PermissionSlug.MANAGE_SHIPMENT)]
        [HttpPost("admin/query")]
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
        [Permission(PermissionSlug.MANAGE_SHIPMENT)]
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
        [Permission(PermissionSlug.MANAGE_SHIPMENT)]
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
        [Permission(PermissionSlug.MANAGE_SHIPMENT)]
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
        [Protected]
        [Permission(PermissionSlug.MANAGE_SHIPMENT)]
        [HttpGet("shipper/{id}")]
        public async Task<IActionResult> GetShipmentsByShipperId(Guid id)
        {
            try
            {
                var result = await _shipmentService.GetShipmentsByShipperId(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}