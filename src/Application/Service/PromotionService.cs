using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KFS.src.Application.Dto.PromotionDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;

namespace KFS.src.Application.Service
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly IProductRepository _productRepository;
        private readonly IBatchRepository _batchRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public PromotionService(IPromotionRepository promotionRepository, IMapper mapper, IProductRepository productRepository, IBatchRepository batchRepository, ICategoryRepository categoryRepository)
        {
            _promotionRepository = promotionRepository;
            _mapper = mapper;
            _productRepository = productRepository;
            _batchRepository = batchRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task<ResponseDto> CreatePromotion(PromotionCreate promotion)
        {
            var response = new ResponseDto();
            try
            {
                var mappedPromotion = _mapper.Map<Promotion>(promotion);
                // set promotion to inactive
                mappedPromotion.IsActive = false;
                var result = await _promotionRepository.CreatePromotion(mappedPromotion);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Promotion created successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Promotion creation failed";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseDto> DeletePromotion(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var promotion = await _promotionRepository.GetPromotionById(id);
                if (promotion == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Promotion not found";
                    response.IsSuccess = false;
                    return response;
                }
                var result = await _promotionRepository.DeletePromotion(promotion);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Promotion deleted successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Promotion deletion failed";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseDto> GetPromotions()
        {
            var response = new ResponseDto();
            try
            {
                var result = await _promotionRepository.GetAllPromotions();
                var mappedPromotions = _mapper.Map<IEnumerable<PromotionDto>>(result);
                if (result != null && mappedPromotions.Count() > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Promotions found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedPromotions
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Promotions not found";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseDto> GetPromotionById(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _promotionRepository.GetPromotionById(id);
                var mappedPromotion = _mapper.Map<PromotionDto>(result);
                if (result != null)
                {
                    response.StatusCode = 200;
                    response.Message = "Promotion found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedPromotion
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Promotion not found";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseDto> ListBatchToPromotion(Guid promotionId, List<Guid> batchId)
        {
            var response = new ResponseDto();
            try
            {
                var promotion = await _promotionRepository.GetPromotionById(promotionId);
                var batchList = new List<Batch>();
                try
                {
                    foreach (var id in batchId)
                    {
                        var batch = await _batchRepository.GetBatchById(id);
                        batchList.Add(batch);
                    }
                    var result = await _promotionRepository.UpdateBatchPromotion(promotion, batchList);
                    if (result)
                    {
                        response.StatusCode = 200;
                        response.Message = "Batch added to promotion successfully";
                        response.IsSuccess = true;
                        return response;
                    }
                    else
                    {
                        response.StatusCode = 400;
                        response.Message = "Batch addition to promotion failed";
                        response.IsSuccess = false;
                        return response;
                    }
                }
                catch
                {
                    throw;
                }

            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseDto> ListCategoryToPromotion(Guid promotionId, List<Guid> categoryId)
        {
            var response = new ResponseDto();
            try
            {
                var promotion = await _promotionRepository.GetPromotionById(promotionId);
                var categoryList = new List<Category>();
                try
                {
                    foreach (var id in categoryId)
                    {
                        var category = await _categoryRepository.GetCategoryById(id);
                        categoryList.Add(category);
                    }
                    var result = await _promotionRepository.UpdateCategoryPromotion(promotion, categoryList);
                    if (result)
                    {
                        response.StatusCode = 200;
                        response.Message = "Category added to promotion successfully";
                        response.IsSuccess = true;
                        return response;
                    }
                    else
                    {
                        response.StatusCode = 400;
                        response.Message = "Category addition to promotion failed";
                        response.IsSuccess = false;
                        return response;
                    }
                }
                catch
                {
                    throw;
                }

            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseDto> ListProductToPromotion(Guid promotionId, List<Guid> productId)
        {
            var response = new ResponseDto();
            try
            {
                var promotion = await _promotionRepository.GetPromotionById(promotionId);
                var productList = new List<Product>();
                try
                {
                    foreach (var id in productId)
                    {
                        var product = await _productRepository.GetProductById(id);
                        productList.Add(product);
                    }
                    var result = await _promotionRepository.UpdateProductPromotion(promotion, productList);
                    if (result)
                    {
                        response.StatusCode = 200;
                        response.Message = "Product added to promotion successfully";
                        response.IsSuccess = true;
                        return response;
                    }
                    else
                    {
                        response.StatusCode = 400;
                        response.Message = "Product addition to promotion failed";
                        response.IsSuccess = false;
                        return response;
                    }
                }
                catch
                {
                    throw;
                }

            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseDto> SetPromotionIsActive(Guid id, bool state)
        {
            var response = new ResponseDto();
            try
            {
                var promotion = await _promotionRepository.GetPromotionById(id);
                if (promotion != null)
                {
                    promotion.IsActive = state;
                    var result = await _promotionRepository.UpdatePromotion(promotion);
                    if (result)
                    {
                        response.StatusCode = 200;
                        response.Message = "Promotion state updated successfully";
                        response.IsSuccess = true;
                        return response;
                    }
                    else
                    {
                        response.StatusCode = 400;
                        response.Message = "Promotion state update failed";
                        response.IsSuccess = false;
                        return response;
                    }
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Promotion not found";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseDto> UpdatePromotion(PromotionUpdate promotion, Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var promotionToUpdate = await _promotionRepository.GetPromotionById(id);
                if (promotionToUpdate == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Promotion not found";
                    response.IsSuccess = false;
                    return response;
                }
                var mappedPromotion = _mapper.Map(promotion, promotionToUpdate);
                var result = await _promotionRepository.UpdatePromotion(mappedPromotion);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Promotion updated successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Promotion update failed";
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