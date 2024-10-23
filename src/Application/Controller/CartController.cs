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
        [HttpGet]
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
        [HttpPost]
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
        [HttpPut("{id}")]
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
        [HttpDelete("{id}")]
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
        [HttpPost("{id}/product/add")]
        public async Task<IActionResult> AddProductToCart(Guid id, CartAddRemoveDto req)
        {
            try
            {
                var result = await _cartService.AddProductToCart(id, req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Protected]
        [HttpPost("{id}/product/remove")]
        public async Task<IActionResult> RemoveProductFromCart(Guid id, CartAddRemoveDto req)
        {
            try
            {
                var result = await _cartService.RemoveProductFromCart(id, req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpPost("{id}/batch/add")]
        public async Task<IActionResult> AddBatchToCart(Guid id, BatchAddRemoveDto req)
        {
            try
            {
                var result = await _cartService.AddBatchToCart(id, req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpPost("{id}/batch/remove")]
        public async Task<IActionResult> RemoveBatchFromCart(Guid id, BatchAddRemoveDto req)
        {
            try
            {
                var result = await _cartService.RemoveBatchFromCart(id, req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetCartByUserId(Guid id)
        {
            try
            {
                var result = await _cartService.GetCartByUserId(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpGet("own")]
        public async Task<IActionResult> GetOwnCart()
        {
            try
            {
                var result = await _cartService.GetOwnCart();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}