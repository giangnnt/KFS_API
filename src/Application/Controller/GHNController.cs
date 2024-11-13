using KFS.src.Application.Dto.GHN;
using KFS.src.Domain.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KFS.src.Application.Controller
{
    [Produces("application/json")]
    [Route("api/GHN")]
    [ApiController]
    public class GHNController : ControllerBase
    {
        private readonly IGHNService _service;
        public GHNController(IGHNService service)
        {
            _service = service;
        }
        [HttpGet("provinces")]
        public async Task<IActionResult> GetProvinces()
        {
            try
            {
                var response = await _service.GetProvinces();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("districts")]
        public async Task<IActionResult> GetDistricts(int provinceId)
        {
            try
            {
                var response = await _service.GetDistricts(provinceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("wards")]
        public async Task<IActionResult> GetWards([FromQuery] int districtId)
        {
            try
            {
                var response = await _service.GetWards(districtId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("calculate-shipping-fee")]
        public async Task<IActionResult> CalculateShippingFee([FromBody] GHNRequest request)
        {
            try
            {
                var response = await _service.CalculateShippingFee(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("services")]
        public async Task<IActionResult> GetServices([FromBody] GHNDeliveryServiceRequest request)
        {
            try
            {
                var response = await _service.GetServices(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
