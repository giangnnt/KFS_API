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
        private readonly IBatchRepository _batchRepository;
        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper, IBatchRepository batchRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _batchRepository = batchRepository;
        }

        public async Task<ResponseDto> CreateProduct(ProductCreate req)
        {
            var response = new ResponseDto();
            try
            {
                //map product
                var mappedProduct = _mapper.Map<Product>(req);
                //check if category id is empty
                if (req.CategoryId == Guid.Empty)
                {
                    response.StatusCode = 400;
                    response.Message = "Category Id is required";
                    response.IsSuccess = false;
                    return response;
                }
                //get category by id
                var Category = await _categoryRepository.GetCategoryById(req.CategoryId);
                //map category
                if (Category != null)
                {
                    mappedProduct.Category = Category;
                }
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
                var mappedProduct = _mapper.Map<ProductDto>(result);
                if (result != null)
                {
                    response.StatusCode = 200;
                    response.Message = "Product found";
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
                if (result != null && result.Count() > 0)
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

        public async Task<ResponseDto> UpdateIsForSell(bool isForSell, Guid id)
        {
            var response = new ResponseDto();
            try
            {
                //get product by id
                var product = await _productRepository.GetProductById(id);
                if (product != null)
                {
                    product.IsForSell = isForSell;
                    foreach (var batch in product.Batches)
                    {
                        batch.IsForSell = isForSell;
                    }
                    var result = _productRepository.UpdateProduct(product);
                    if (result.Result)
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

        public async Task<ResponseDto> UpdateProduct(ProductUpdate req, Guid id)
        {
            var response = new ResponseDto();
            try
            {
                //get product by id
                var product = await _productRepository.GetProductById(id);
                //map product
                var mappedProduct = _mapper.Map(req, product);
                if (req.CategoryId != null)
                {
                    var Category = await _categoryRepository.GetCategoryById(req.CategoryId.Value);
                    mappedProduct.CategoryId = req.CategoryId.Value;
                    mappedProduct.Category = Category;
                    if (Category == null)
                    {
                        response.StatusCode = 404;
                        response.Message = "Category not found";
                        response.IsSuccess = false;
                        return response;
                    }
                }
                //update product
                var result = await _productRepository.UpdateProduct(product);
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
    }
}