using KFS.src.Application.Constant;
using KFS.src.Application.Dto.CategoryDtos;
using KFS.src.Application.Middleware;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IService;
using Microsoft.AspNetCore.Mvc;

namespace KFS.src.Application.Controller
{
    [Produces("application/json")]
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var result = await _categoryService.GetCategories();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_CATEGORY)]
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryCreate req)
        {
            try
            {
                var result = await _categoryService.CreateCategory(req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            try
            {
                var result = await _categoryService.GetCategoryById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_CATEGORY)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            try
            {
                var result = await _categoryService.DeleteCategory(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_CATEGORY)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(CategoryUpdate req, Guid id)
        {
            try
            {
                var result = await _categoryService.UpdateCategory(req, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetProductByCategory(Guid id)
        {
            try
            {
                var result = await _categoryService.GetProductByCategory(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [Permission(PermissionSlug.MANAGE_CATEGORY)]
        [HttpPut("{id}/products")]
        public async Task<IActionResult> UpdateProductCategory(Guid id, List<Guid> productId)
        {
            try
            {
                var result = await _categoryService.UpdateProductCategory(id, productId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
