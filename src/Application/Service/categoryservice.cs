﻿using AutoMapper;
using KFS.src.Application.Dto.CategoryDtos;
using KFS.src.Application.Dto.ProductDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;
using KFS.src.Infrastucture.Repository;
using Microsoft.AspNetCore.Mvc;

namespace KFS.src.Application.Service
{
    public class categoryservice : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public  categoryservice(ICategoryRepository Caterepo, IMapper mapper)
        {
           
            _categoryRepository = Caterepo;
            _mapper = mapper;
        }

        public async Task<ResponseDto> CreateCategory(categorydtov2 category)
        {
            var response = new ResponseDto();
            try
            {
                //map product
                var mappedCategory = _mapper.Map<Category>(category);
                //check if category id is empty
               mappedCategory.Id=Guid.NewGuid();
               
                //get category by id
                
                //map category
                
                var result = await _categoryRepository.CreateCategory(mappedCategory);
                if (result)
                {
                    response.StatusCode = 201;
                    response.Message = "Product created successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Product creation failed";
                    response.IsSuccess = false;
                    return response;
                }

            }
            catch
            {
                throw;
            }
        }

       

        public async Task<ResponseDto> DeleteCategory(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _categoryRepository.DeleteCategory(id);
                if (result)


                {
                    response.StatusCode = 200;
                    response.Message = "Product deleted successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Product not found";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseDto> GetAllCategories()
        {
            var response = new ResponseDto();
            try
            {
                var result = await _categoryRepository.GetCategories();
                var mappedcategory = _mapper.Map<List<Category>>(result);
                if (result != null && result.Count() > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Category found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedcategory
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Products not found";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch
            {
                throw;
            }
        }
        [HttpGet ("id")]
        public async Task<ResponseDto> GetCategoryById(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _categoryRepository.GetCategoryById(id);
                var mappedProduct = _mapper.Map<Category>(result);
                if (result != null)
                {
                    response.StatusCode = 200;
                    response.Message = "Category found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedProduct
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
            catch
            {
                throw;
            }
        }

        public async Task<ResponseDto> GetCategoryByName(string name)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _categoryRepository.GetCategoryByName(name);
                var mappedProduct = _mapper.Map<Category>(result);
                if (result != null)
                {
                    response.StatusCode = 200;
                    response.Message = "Category found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedProduct
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
            catch
            {
                throw;
            }
        }

       

        public async Task<ResponseDto> UpdateCategory(categoryv3 category, Guid id)
        {
            var response = new ResponseDto();
            try
            {
                //get product by id
                var categoryy = await _categoryRepository.GetCategoryById(id);
                //map product
                var mappedProduct = _mapper.Map(category,categoryy);


                if (categoryy == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Category not found";
                    response.IsSuccess = false;
                    return response;
                }

                //update product
                var result = await _categoryRepository.UpdateCategory(categoryy);
                //check resultF
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Product updated successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Product update failed";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch
            {
                throw;
            }
        }



        //public async Task<ResponseDto> UpdateCategory(Guid id ,categorydtov2 category)
        //{
        //    var response = new ResponseDto();
        //    try
        //    {
        //        //get product by id
        //        var product = await _categoryRepository.GetCategoryById(id);
        //        //map product
        //        var mappedCate = _mapper.Map<Category>(category);
        //        mappedCate.Id=Guid.NewGuid(); 
        //        if (req.C != null)
        //        {
        //            var Category = await _categoryRepository.GetCategoryById(req.CategoryId.Value);
        //            mappedProduct.CategoryId = req.CategoryId.Value;
        //            mappedProduct.Category = Category;
        //            if (Category == null)
        //            {
        //                response.StatusCode = 404;
        //                response.Message = "Category not found";
        //                response.IsSuccess = false;
        //                return response;
        //            }
        //        }
        //        //update product
        //        var result = await _productRepository.UpdateProduct(product);
        //        //check resultF
        //        if (result)
        //        {
        //            response.StatusCode = 200;
        //            response.Message = "Product updated successfully";
        //            response.IsSuccess = true;
        //            return response;
        //        }
        //        else
        //        {
        //            response.StatusCode = 400;
        //            response.Message = "Product update failed";
        //            response.IsSuccess = false;
        //            return response;
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}


    }
}
