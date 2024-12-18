using KFS.src.Application.Constant;
using KFS.src.Application.Dto.ProductDtos;
using KFS.src.Application.Middleware;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IService;
using Microsoft.AspNetCore.Mvc;

namespace KFS.src.Application.Controller
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpPost("query")]
        public async Task<IActionResult> GetProducts(ProductQuery productQuery)
        {
            try
            {
                var result = await _productService.GetProducts(productQuery);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            try
            {
                var result = await _productService.GetProductById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_PRODUCT)]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreate req)
        {
            try
            {
                var result = await _productService.CreateProduct(req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_PRODUCT)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdate req, Guid id)
        {
            try
            {
                var result = await _productService.UpdateProduct(req, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_PRODUCT)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                var result = await _productService.DeleteProduct(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_PRODUCT)]
        [HttpPut("{id}/is-for-sell")]
        public async Task<IActionResult> UpdateIsForSell(Guid id, [FromQuery] bool isForSell)
        {
            try
            {
                var result = await _productService.UpdateIsForSell(isForSell, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_PRODUCT, PermissionSlug.VIEW_PRODUCT)]
        [HttpPost("admin/query")]
        public async Task<IActionResult> GetProductsAdmin(ProductAdminQuery productQuery)
        {
            try
            {
                var result = await _productService.GetProductsAdmin(productQuery);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_PRODUCT)]
        [HttpPut("{id}/is-active")]
        public async Task<IActionResult> UpdateProductIsActive(Guid id, [FromQuery] bool isActive)
        {
            try
            {
                var result = await _productService.UpdateProductIsActive(isActive, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}