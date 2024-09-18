using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.OrderDtos;
using KFS.src.Domain.IService;
using Microsoft.AspNetCore.Mvc;

namespace KFS.src.Application.Controller
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var result = await _orderService.GetOrders();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            try
            {
                var result = await _orderService.GetOrderById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("create-from-cart")]
        public async Task<IActionResult> CreateOrderFromCart(OrderCreateFromCart req)
        {
            try
            {
                var result = await _orderService.CreateOrderFromCart(req);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, OrderUpdate req)
        {
            try
            {
                var result = await _orderService.UpdateOrder(req, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            try
            {
                var result = await _orderService.DeleteOrder(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("vnpayment-return")]
        public async Task<IActionResult> PaymentReturn()
        {
            try
            {
                var result = await _orderService.GetResponsePaymentUrl();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}