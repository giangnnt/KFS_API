using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.PaymentDtos;
using KFS.src.Application.Middleware;
using KFS.src.Domain.IService;
using Microsoft.AspNetCore.Mvc;

namespace KFS.src.Application.Controller
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [Protected]
        [HttpPost("admin/query")]
        public async Task<IActionResult> GetPayments(PaymentQuery paymentQuery)
        {
            try
            {
                var result = await _paymentService.GetPayments(paymentQuery);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(Guid id)
        {
            try
            {
                var result = await _paymentService.GetPaymentById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpPost("offline-order/{id}")]
        public async Task<IActionResult> CreatePaymentOffline(Guid id)
        {
            try
            {
                var result = await _paymentService.CreatePaymentOffline(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(Guid id)
        {
            try
            {
                var result = await _paymentService.DeletePayment(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpPost("COD-order/{id}")]
        public async Task<IActionResult> CreatePaymentByOrderId(Guid id)
        {
            try
            {
                var result = await _paymentService.CreatePaymentByOrderIdCOD(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetPaymentByUser(Guid id)
        {
            try
            {
                var result = await _paymentService.GetPaymentByUser(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpGet("own")]
        public async Task<IActionResult> GetOwnPayment()
        {
            try
            {
                var result = await _paymentService.GetOwnPayment();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}