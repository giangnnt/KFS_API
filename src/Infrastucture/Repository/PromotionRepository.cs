using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Infrastucture.Context;
using Microsoft.EntityFrameworkCore;

namespace KFS.src.Infrastucture.Repository
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly KFSContext _context;
        public PromotionRepository(KFSContext context)
        {
            _context = context;
        }
        public async Task<bool> CreatePromotion(Promotion promotion)
        {
            _context.Promotions.Add(promotion);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeletePromotion(Promotion promotion)
        {
            _context.Promotions.Remove(promotion);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<Promotion>> GetAllPromotions()
        {
            return await _context.Promotions
            .Include(x => x.Products)
            .Include(x => x.Categories)
            .Include(x => x.Batches)
            .ToListAsync();
        }

        public async Task<Promotion> GetPromotionById(Guid id)
        {
            return await _context.Promotions
            .Include(x => x.Products)
            .Include(x => x.Categories)
            .Include(x => x.Batches)
            .FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Promotion not found");
        }

        public async Task<bool> UpdateBatchPromotion(Promotion promotion, List<Batch> batch)
        {
            // get and remove batch
            foreach (var item in promotion.Batches)
            {
                if (!batch.Contains(item))
                {
                    promotion.Batches.Remove(item);
                }
            }
            // add new batch
            foreach (var item in batch)
            {
                if (!promotion.Batches.Contains(item))
                {
                    promotion.Batches.Add(item);
                }
            }
            _context.Promotions.Update(promotion);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateCategoryPromotion(Promotion promotion, List<Category> category)
        {
            // get and remove category
            foreach (var item in promotion.Categories)
            {
                if (!category.Contains(item))
                {
                    promotion.Categories.Remove(item);
                }
            }
            // add new category
            foreach (var item in category)
            {
                if (!promotion.Categories.Contains(item))
                {
                    promotion.Categories.Add(item);
                }
            }
            _context.Promotions.Update(promotion);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateProductPromotion(Promotion promotion, List<Product> product)
        {
            // get and remove product
            for (int i = 0; i < promotion.Products.Count; i++)
            {
                if (!product.Contains(promotion.Products[i]))
                {
                    promotion.Products.RemoveAt(i);
                }
            }
            // add new product
            foreach (var item in product)
            {
                if (!promotion.Products.Contains(item))
                {
                    promotion.Products.Add(item);
                }
            }
            _context.Promotions.Update(promotion);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdatePromotion(Promotion promotion)
        {
            _context.Promotions.Update(promotion);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}