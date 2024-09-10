using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KFS.src.Application.Dto.ProductDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;

namespace KFS.src.Application.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDto> CreateProduct(ProductCreate req)
        {
            var response = new ResponseDto();
            try
            {
                if (req.CategoryId == Guid.Empty)
                {
                    response.StatusCode = 400;
                    response.Message = "Category Id is required";
                    response.IsSuccess = false;
                    return response;
                }
                var Category = await _categoryRepository.GetCategoryById(req.CategoryId);
                var mappedProduct = _mapper.Map<Product>(req);
                mappedProduct.Category = Category;
                var result = await _productRepository.CreateProduct(mappedProduct);
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

        public async Task<ResponseDto> DeleteProduct(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _productRepository.DeleteProduct(id);
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

        public async Task<ResponseDto> GetProductById(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _productRepository.GetProductById(id);
                if (result != null)
                {
                    response.StatusCode = 200;
                    response.Message = "Product found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = result
                    };
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

        public async Task<ResponseDto> GetProducts()
        {
            var response = new ResponseDto();
            try
            {
                var result = await _productRepository.GetProducts();
                var mappedProduct = _mapper.Map<List<ProductDto>>(result);
                if (result != null)
                {
                    response.StatusCode = 200;
                    response.Message = "Products found";
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

        public async Task<ResponseDto> UpdateProduct(Product product)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _productRepository.UpdateProduct(product);
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
    }
}