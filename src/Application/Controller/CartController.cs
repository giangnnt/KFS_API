using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Constant;
using KFS.src.Application.Dto.CartDtos;
using KFS.src.Application.Middleware;
using KFS.src.Domain.IService;
using Microsoft.AspNetCore.Mvc;

namespace KFS.src.Application.Controller
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Protected]
        [HttpGet("all")]
        public async Task<IActionResult> GetCarts()
        {
            try
            {
                var result = await _cartService.GetCarts();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCartById(Guid id)
        {
            try
            {
                var result = await _cartService.GetCartById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Protected]
        [HttpPost("create")]
        public async Task<IActionResult> CreateCart(CartCreate req)
        {
            try
            {
                var result = await _cartService.CreateCart(req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Protected]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCart(CartUpdate req, Guid id)
        {
            try
            {
                var result = await _cartService.UpdateCart(req, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Protected]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCart(Guid id)
        {
            try
            {
                var result = await _cartService.DeleteCart(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Protected]
        [HttpPost("add-product")]
        public async Task<IActionResult> AddProductToCart(CartAddRemoveDto req)
        {
            try
            {
                var result = await _cartService.AddProductToCart(req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Protected]
        [HttpPost("remove-product")]
        public async Task<IActionResult> RemoveProductFromCart(CartAddRemoveDto req)
        {
            try
            {
                var result = await _cartService.RemoveProductFromCart(req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpPost("add-batch")]
        public async Task<IActionResult> AddBatchToCart(BatchAddRemoveDto req)
        {
            try
            {
                var result = await _cartService.AddBatchToCart(req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpPost("remove-batch")]
        public async Task<IActionResult> RemoveBatchFromCart(BatchAddRemoveDto req)
        {
            try
            {
                var result = await _cartService.RemoveBatchFromCart(req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetCartByUserId(Guid userId)
        {
            try
            {
                var result = await _cartService.GetCartByUserId(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}