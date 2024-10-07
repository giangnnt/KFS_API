using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Domain.Entities;

namespace KFS.src.Domain.IRepository
{
    public interface IPromotionRepository
    {
        public Task<IEnumerable<Promotion>> GetAllPromotions();
        public Task<Promotion> GetPromotionById(Guid id);
        public Task<bool> CreatePromotion(Promotion promotion);
        public Task<bool> UpdatePromotion(Promotion promotion);
        public Task<bool> DeletePromotion(Promotion promotion);
        public Task<bool> UpdateBatchPromotion(Promotion promotion, List<Batch> batch);
        public Task<bool> UpdateCategoryPromotion(Promotion promotion, List<Category> category);
        public Task<bool> UpdateProductPromotion(Promotion promotion, List<Product> product);
    }
}