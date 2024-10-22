using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Constant;
using KFS.src.Application.Dto.OrderDtos;
using KFS.src.Application.Enum;
using KFS.src.Application.Middleware;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IService;
using Microsoft.AspNetCore.Mvc;

namespace KFS.src.Application.Controller
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [Protected]
        [HttpGet("all")]
        public async Task<IActionResult> GetOrders(OrderQuery req)
        {
            try
            {
                var result = await _orderService.GetOrders(req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
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
        [Protected]
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
        [Protected]
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
        [Protected]
        [HttpPut("update-status/{id}")]
        public async Task<IActionResult> UpdateOrderStatus(OrderUpdateStatus req)
        {
            try
            {
                var result = await _orderService.UpdateOrderStatus(req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrderByUserId(Guid userId)
        {
            try
            {
                var result = await _orderService.GetOrderByUserId(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpPost("create-offline")]
        public async Task<IActionResult> CreateOrderOffline(OrderCreateOffline req)
        {
            try
            {
                var result = await _orderService.CreateOrderOffline(req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpGet("get-own-order")]
        public async Task<IActionResult> GetOwnOrder()
        {
            try
            {
                var result = await _orderService.GetOwnOrder();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}