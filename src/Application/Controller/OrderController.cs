using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        [Permission(PermissionSlug.MANAGE_ORDER, PermissionSlug.VIEW_ORDER)]
        [HttpPost("admin/query")]
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
        [Permission(PermissionSlug.MANAGE_ORDER, PermissionSlug.VIEW_ORDER)]
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
        [Protected]
        [Permission(PermissionSlug.MANAGE_ORDER, PermissionSlug.MANAGE_OWN_ORDER)]
        [HttpPost("cart/{id}")]
        public async Task<IActionResult> CreateOrderFromCart(Guid id, OrderCreateFromCart req)
        {
            try
            {
                var result = await _orderService.CreateOrderFromCart(id, req);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // [Protected]
        // [HttpPut("{id}")]
        // public async Task<IActionResult> UpdateOrder(Guid id, OrderUpdate req)
        // {
        //     try
        //     {
        //         var result = await _orderService.UpdateOrder(req, id);
        //         return Ok(result);
        //     }
        //     catch (Exception ex)
        //     {
        //         return BadRequest(ex.Message);
        //     }
        // }
        [Protected]
        [Permission(PermissionSlug.MANAGE_ORDER, PermissionSlug.MANAGE_OWN_ORDER)]
        [HttpDelete("{id}")]
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
                return Redirect(result.Result.Data.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_ORDER, PermissionSlug.UPDATE_ORDER)]
        [HttpPut("{id}/is-accept")]
        public async Task<IActionResult> AcceptOrder(Guid id, [FromQuery] bool isAccept)
        {
            try
            {
                var result = await _orderService.AcceptOrder(id, isAccept);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_ORDER, PermissionSlug.VIEW_ORDER)]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetOrderByUserId(Guid id)
        {
            try
            {
                var result = await _orderService.GetOrderByUserId(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_ORDER, PermissionSlug.CREATE_ORDER_OFFLINE)]
        [HttpPost("offline")]
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
        [Permission(PermissionSlug.MANAGE_ORDER, PermissionSlug.MANAGE_OWN_ORDER)]
        [HttpGet("own")]
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
        [Protected]
        [Permission(PermissionSlug.MANAGE_ORDER, PermissionSlug.MANAGE_OWN_ORDER)]
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(Guid id)
        {
            try
            {
                var result = await _orderService.CancelOrder(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_ORDER, PermissionSlug.MANAGE_OWN_ORDER)]
        [HttpPut("{id}/return")]
        public async Task<IActionResult> OrderReturn(Guid id)
        {
            try
            {
                var result = await _orderService.OrderReturn(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}