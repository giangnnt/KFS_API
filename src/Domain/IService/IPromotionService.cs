using KFS.src.Application.Dto.PromotionDtos;
using KFS.src.Application.Dto.ResponseDtos;

namespace KFS.src.Domain.IService
{
    public interface IPromotionService
    {
        public Task<ResponseDto> GetPromotions();
        public Task<ResponseDto> GetPromotionById(Guid id);
        public Task<ResponseDto> CreatePromotion(PromotionCreate promotion);
        public Task<ResponseDto> UpdatePromotion(PromotionUpdate promotion, Guid id);
        public Task<ResponseDto> DeletePromotion(Guid id);
        public Task<ResponseDto> StartPromotion(Guid id);
        public Task<ResponseDto> UpdateProductToPromotion(Guid promotionId, List<Guid> productId);
        public Task<ResponseDto> UpdateBatchToPromotion(Guid promotionId, List<Guid> batchId);
        public Task<ResponseDto> UpdateCategoryToPromotion(Guid promotionId, List<Guid> categoryId);
        public Task<ResponseDto> EndPromotion(Guid promotionId);
    }
}