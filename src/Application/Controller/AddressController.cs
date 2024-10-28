using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.AddressDtos;
using KFS.src.Application.Middleware;
using KFS.src.Domain.IService;
using Microsoft.AspNetCore.Mvc;

namespace KFS.src.Application.Controller
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/address")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }
        [Protected]
        [HttpPost]
        public async Task<IActionResult> CreateAddress([FromBody] AddressCreate addressCreate)
        {
            var response = await _addressService.CreateAddress(addressCreate);
            return Ok(response);
        }
        [Protected]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(Guid id)
        {
            var response = await _addressService.DeleteAddress(id);
            return Ok(response);
        }
        [Protected]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddressById(Guid id)
        {
            var response = await _addressService.GetAddressById(id);
            return Ok(response);
        }
        [Protected]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetAddressByUserId(Guid id)
        {
            var response = await _addressService.GetAddressByUserId(id);
            return Ok(response);
        }
        [Protected]
        [HttpGet("own")]
        public async Task<IActionResult> GetAddressOwn()
        {
            var response = await _addressService.GetAddressOwn();
            return Ok(response);
        }
    }
}