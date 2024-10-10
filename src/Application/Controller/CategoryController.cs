using KFS.src.Application.Dto.CategoryDtos;
using KFS.src.Application.Dto.ProductDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Application.Middleware;
using KFS.src.Application.Service;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KFS.src.Application.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        //Task<ResponseDto> GetAllCategories();
        //Task<ResponseDto> GetCategoryById(int id);
        //Task<ResponseDto> CreateCategory(Category category);
        //Task<ResponseDto> UpdateCategory(Category category);
        //Task<ResponseDto> DeleteCategory(int id);
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]

        public async Task<IActionResult> GetCategory()
        {
            try
            {
                var result = await _categoryService.GetAllCategories();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("name")]
        public async Task<IActionResult> GetCategoryByName(string name)
        {
            try
            {
                var result = await _categoryService.GetCategoryByName(name);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory([FromBody] categorydtov2 req)
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
        [HttpGet("Id")]
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById([FromRoute] Guid id)
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
        [HttpPut ("{id}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid id,[FromBody] categoryv3 req)
        {
            try
            {
                var result = await _categoryService.UpdateCategory(req,id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
