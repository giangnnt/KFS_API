using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using KFS.src.Application.Dto.PromotionDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Domain.Entities;

namespace KFS.src.Domain.IService
{
    public interface IPromotionService
    {
        public Task<ResponseDto> GetPromotions();
        public Task<ResponseDto> GetPromotionById(Guid id);
        public Task<ResponseDto> CreatePromotion(PromotionCreate promotion);
        public Task<ResponseDto> UpdatePromotion(PromotionUpdate promotion, Guid id);
        public Task<ResponseDto> DeletePromotion(Guid id);
        public Task<ResponseDto> SetPromotionIsActive(Guid id, bool state);
        public Task<ResponseDto> ListProductToPromotion(Guid promotionId, List<Guid> productId);
        public Task<ResponseDto> ListBatchToPromotion(Guid promotionId, List<Guid> batchId);
        public Task<ResponseDto> ListCategoryToPromotion(Guid promotionId, List<Guid> categoryId);

    }
}