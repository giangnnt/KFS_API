using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KFS.src.Application.Dto.CategoryDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;

namespace KFS.src.Application.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public Task<ResponseDto> CreateCategory(CategoryCreate category)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> DeleteCategory(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto> GetCategories()
        {
            var response = new ResponseDto();
            try
            {
                var categories = await _categoryRepository.GetCategories();
                if (categories != null)
                {
                    response.StatusCode = 200;
                    response.Message = "Categories retrieved successfully";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = categories
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "No category found";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> GetCategoryById(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var category = await _categoryRepository.GetCategoryById(id);
                if (category != null)
                {
                    response.StatusCode = 200;
                    response.Message = "Category found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = category
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Category not found";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public Task<ResponseDto> UpdateCategory(CategoryUpdate category, Guid id)
        {
            throw new NotImplementedException();
        }
    }
}