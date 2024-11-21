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
        [Permission(PermissionSlug.MANAGE_CART, PermissionSlug.VIEW_CART)]
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
        [Protected]
        [Permission(PermissionSlug.MANAGE_CART, PermissionSlug.VIEW_CART)]
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
        [Permission(PermissionSlug.MANAGE_CART, PermissionSlug.MANAGE_OWN_CART)]
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
        [Permission(PermissionSlug.MANAGE_CART, PermissionSlug.MANAGE_OWN_CART)]
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
        [Permission(PermissionSlug.MANAGE_CART, PermissionSlug.MANAGE_OWN_CART)]
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
        [Permission(PermissionSlug.MANAGE_CART, PermissionSlug.MANAGE_OWN_CART)]
        [HttpPost("{id}/product/{productId}/add")]
        public async Task<IActionResult> AddProductToCart(Guid id, Guid productId)
        {
            try
            {
                var result = await _cartService.AddProductToCart(id, productId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Protected]
        [Permission(PermissionSlug.MANAGE_CART, PermissionSlug.MANAGE_OWN_CART)]
        [HttpPost("{id}/cart-item/{itemId}/remove")]
        public async Task<IActionResult> RemoveItemFromCart(Guid id, Guid itemId)
        {
            try
            {
                var result = await _cartService.RemoveItemCart(id, itemId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_CART, PermissionSlug.MANAGE_OWN_CART)]
        [HttpPost("{id}/batch/{batchId}/add")]
        public async Task<IActionResult> AddBatchToCart(Guid id, Guid batchId)
        {
            try
            {
                var result = await _cartService.AddBatchToCart(id, batchId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_CART, PermissionSlug.MANAGE_OWN_CART, PermissionSlug.VIEW_CART)]
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
        [Permission(PermissionSlug.MANAGE_CART, PermissionSlug.MANAGE_OWN_CART)]
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
        [Protected]
        [Permission(PermissionSlug.MANAGE_CART, PermissionSlug.MANAGE_OWN_CART)]
        [HttpGet("own/current-active")]
        public async Task<IActionResult> GetOwnCartCurrentActive()
        {
            try
            {
                var result = await _cartService.GetOwnCartCurrentActive();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}