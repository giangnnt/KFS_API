using AutoMapper;
using Hangfire;
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
        private readonly IBackgroundJobClient _backgroundJobClient;
        public PromotionService(IPromotionRepository promotionRepository, IMapper mapper, IProductRepository productRepository, IBatchRepository batchRepository, ICategoryRepository categoryRepository, IBackgroundJobClient backgroundJobClient)
        {
            _promotionRepository = promotionRepository;
            _mapper = mapper;
            _productRepository = productRepository;
            _batchRepository = batchRepository;
            _categoryRepository = categoryRepository;
            _backgroundJobClient = backgroundJobClient;
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
                    _backgroundJobClient.Schedule(() => JobStartPromotion(mappedPromotion.Id), mappedPromotion.StartDate);
                    _backgroundJobClient.Schedule(() => JobExpirePromotion(mappedPromotion.Id), mappedPromotion.EndDate);
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
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
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
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
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
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
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
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> UpdateBatchToPromotion(Guid promotionId, List<Guid> batchId)
        {
            var response = new ResponseDto();
            try
            {
                var promotion = await _promotionRepository.GetPromotionById(promotionId);
                var batchList = new List<Batch>();

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
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> UpdateCategoryToPromotion(Guid promotionId, List<Guid> categoryId)
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
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> UpdateProductToPromotion(Guid promotionId, List<Guid> productId)
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
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> StartPromotion(Guid id)
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
                // set new price for products and batches
                foreach (var product in promotion.Products)
                {
                    if (product.Status != Enum.ProductStatusEnum.InBatch)
                    {
                        product.Price = product.Price - (product.Price * promotion.DiscountPercentage / 100);
                    }
                }
                foreach (var batch in promotion.Batches)
                {
                    batch.Price = batch.Price - (batch.Price * promotion.DiscountPercentage / 100);
                }
                // set new price for products in category except the one already in promotion
                var productInCategory = promotion.Categories.SelectMany(x => x.Products).ToList();
                foreach (var product in productInCategory)
                {
                    if (!promotion.Products.Contains(product))
                    {
                        product.Price = product.Price - (product.Price * promotion.DiscountPercentage / 100);
                    }
                }
                // set promotion state
                promotion.IsActive = true;
                // update promotion
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
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
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
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task JobExpirePromotion(Guid promotionId)
        {
            var response = new ResponseDto();
            try
            {
                var promotion = await _promotionRepository.GetPromotionById(promotionId);
                if (promotion == null)
                {
                    Console.WriteLine("Promotion not found");
                    return;
                }
                // set new price for products and batches
                foreach (var product in promotion.Products)
                {
                    if (product.Status != Enum.ProductStatusEnum.InBatch)
                    {
                        product.Price = product.Price * 100 / (100 - promotion.DiscountPercentage);
                    }
                }
                foreach (var batch in promotion.Batches)
                {
                    batch.Price = batch.Price * 100 / (100 - promotion.DiscountPercentage);
                }
                // set new price for products and batches in category except the one already in promotion
                var productInCategory = promotion.Categories.SelectMany(x => x.Products).ToList();
                foreach (var product in productInCategory)
                {
                    if (!promotion.Products.Contains(product))
                    {
                        product.Price = product.Price * 100 / (100 - promotion.DiscountPercentage);
                    }
                }
                // set promotion state
                promotion.IsActive = false;
                // update promotion
                var result = await _promotionRepository.UpdatePromotion(promotion);
                if (result)
                {
                    Console.WriteLine("Promotion expired successfully");
                    return;
                }
                else
                {
                    Console.WriteLine("Promotion expiration failed");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }
        public async Task JobStartPromotion(Guid promotionId)
        {
            var response = new ResponseDto();
            try
            {
                var promotion = await _promotionRepository.GetPromotionById(promotionId);
                if (promotion == null)
                {
                    Console.WriteLine("Promotion not found");
                    return;
                }
                // set new price for products and batches
                foreach (var product in promotion.Products)
                {
                    if (product.Status != Enum.ProductStatusEnum.InBatch)
                    {
                        product.Price = product.Price - (product.Price * promotion.DiscountPercentage / 100);
                    }
                }
                foreach (var batch in promotion.Batches)
                {
                    batch.Price = batch.Price - (batch.Price * promotion.DiscountPercentage / 100);
                }
                // set new price for products and batches in category except the one already in promotion
                var productInCategory = promotion.Categories.SelectMany(x => x.Products).ToList();
                foreach (var product in productInCategory)
                {
                    if (!promotion.Products.Contains(product))
                    {
                        product.Price = product.Price - (product.Price * promotion.DiscountPercentage / 100);
                    }
                }
                // set promotion state
                promotion.IsActive = true;
                // update promotion
                var result = await _promotionRepository.UpdatePromotion(promotion);
                if (result)
                {
                    Console.WriteLine("Promotion started successfully");
                    return;
                }
                else
                {
                    Console.WriteLine("Promotion start failed");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }

        public async Task<ResponseDto> EndPromotion(Guid promotionId)
        {
            var response = new ResponseDto();
            try
            {
                var promotion = await _promotionRepository.GetPromotionById(promotionId);
                if (promotion == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Promotion not found";
                    response.IsSuccess = false;
                    return response;
                }
                // set new price for products and batches
                foreach (var product in promotion.Products)
                {
                    if (product.Status != Enum.ProductStatusEnum.InBatch)
                    {
                        product.Price = product.Price * 100 / (100 - promotion.DiscountPercentage);
                    }
                }
                foreach (var batch in promotion.Batches)
                {
                    batch.Price = batch.Price * 100 / (100 - promotion.DiscountPercentage);
                }
                // set new price for products and batches in category except the one already in promotion
                var productInCategory = promotion.Categories.SelectMany(x => x.Products).ToList();
                foreach (var product in productInCategory)
                {
                    if (!promotion.Products.Contains(product))
                    {
                        product.Price = product.Price * 100 / (100 - promotion.DiscountPercentage);
                    }
                }
                // set promotion state
                promotion.IsActive = false;
                // update promotion
                var result = await _promotionRepository.UpdatePromotion(promotion);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Promotion ended successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Promotion end failed";
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
    }
}