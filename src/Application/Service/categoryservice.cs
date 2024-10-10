using AutoMapper;
using KFS.src.Application.Dto.CategoryDtos;
using KFS.src.Application.Dto.ProductDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;
using KFS.src.Infrastucture.Repository;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace KFS.src.Application.Service
{
    public class categoryservice : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public categoryservice(ICategoryRepository Caterepo, IMapper mapper)
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
                mappedCategory.Id = Guid.NewGuid();

                //get category by id

                //map category

                var result = await _categoryRepository.CreateCategory(mappedCategory);
                if (result)
                {
                    response.StatusCode = 201;
                    response.Message = "Category created successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Category creation failed";
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
                    response.Message = "Category deleted successfully";
                    response.IsSuccess = true;
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

        public async Task<ResponseDto> GetAllCategories()
        {

            var response = new ResponseDto();
            try
            {
                var result = await _categoryRepository.GetCategories();
                var mappedCategories = _mapper.Map<List<CategoryDto>>(result);

                if (mappedCategories != null && mappedCategories.Any())
                {
                    // Group products by category
                    var categoriesWithProducts = mappedCategories.Select(category => new
                    {
                        CategoryId = category.Id,
                        CategoryName = category.Name,
                        CategoryDescription = category.Description,
                        Products = category.Products.Select(product => new
                        {
                            ProductId = product.Id,
                            ProductName = product.Name,
                            ProductDescription = product.Description,
                            Price = product.Price,
                            // Add other product properties as needed
                        }).ToList()
                    }).ToList();

                    response.StatusCode = 200;
                    response.Message = "Categories with products found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = categoriesWithProducts,
                    };

                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Categories not found";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = "An error occurred while fetching categories";
                response.IsSuccess = false;
                return response;
            }
        }

        [HttpGet("id")]
        public async Task<ResponseDto> GetCategoryById(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _categoryRepository.GetCategoryById(id);

                if (result != null)
                {
                    var mappedCategory = _mapper.Map<CategoryDto>(result);

                    var categoryWithProducts = new
                    {
                        CategoryId = mappedCategory.Id,
                        CategoryName = mappedCategory.Name,
                        CategoryDescription = mappedCategory.Description,
                        Products = mappedCategory.Products.Select(product => new
                        {
                            ProductId = product.Id,
                            ProductName = product.Name,
                            ProductDescription = product.Description,
                            Price = product.Price,
                            // Add other product properties as needed
                        }).ToList()
                    };

                    response.StatusCode = 200;
                    response.Message = "Category found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = categoryWithProducts
                    };
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Category not found";
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = "An error occurred while fetching the category";
                response.IsSuccess = false;

            }

            return response;
        }

        public async Task<ResponseDto> GetCategoryByName(string name)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _categoryRepository.GetCategoryByName(name);

                if (result != null)
                {
                    var mappedCategory = _mapper.Map<CategoryDto>(result);

                    var categoryWithProducts = new
                    {
                        CategoryId = mappedCategory.Id,
                        CategoryName = mappedCategory.Name,
                        CategoryDescription = mappedCategory.Description,
                        Products = mappedCategory.Products.Select(product => new
                        {
                            ProductId = product.Id,
                            ProductName = product.Name,
                            ProductDescription = product.Description,
                            Price = product.Price,
                            // Add other product properties as needed
                        }).ToList()
                    };

                    response.StatusCode = 200;
                    response.Message = "Category found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = categoryWithProducts
                    };
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Category not found";
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = "An error occurred while fetching the category";
                response.IsSuccess = false;

            }

            return response;
        }

      

        public async Task<ResponseDto> GetProductByCateId(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var products = await _categoryRepository.GetProductBy(id);
                if (products == null || !products.Any())
                {
                    response.StatusCode = 404;
                    response.Message = "No products found for this category";
                    response.IsSuccess = false;
                    return response;
                }

                var mappedProducts = _mapper.Map<List<ProductDto1>>(products);
                var simplifiedProducts = mappedProducts.Select(p => new
                {
                    id = p.Id,
                    ProductName = p.Name,
                    species = p.Species,
                    length = p.Length,
                    price = p.Price,
                    origin = p.Origin,
                    age = p.Age,
                    color = p.Color,
                    feedingvollume = p.FeedingVolumn,
                    filter_rate = p.FilterRate,
                    gender = p.Gender,
                    inventory = p.Inventory,
                    isforsell = p.IsForSell
                }).ToList();

                response.StatusCode = 200;
                response.Message = "Products found";
                response.IsSuccess = true;
                response.Result = new ResultDto
                {
                    Data = simplifiedProducts
                };
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = "An error occurred while fetching the products";
                response.IsSuccess = false;
                // Consider logging the exception here
            }
            return response;
        }

        public async Task<ResponseDto> UpdateCategory(categoryv3 category, Guid id)
        {
            var response = new ResponseDto();

            try { 
            //get product by id
            var categoryy = await _categoryRepository.GetCategoryById(id);
            //map product
            var mappedProduct = _mapper.Map(category, categoryy);
            
           

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
                    response.Message = "Category updated successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Category update failed";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch
            {
                throw;
            }

        }
        public async Task<ResponseDto> DeleteProductByProId(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _categoryRepository.DeleteProduct(id);
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

        
    }
    }
   

    
