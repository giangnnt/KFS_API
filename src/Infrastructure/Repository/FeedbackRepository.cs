using KFS.src.Application.Dto.FeedbackDtos;
using KFS.src.Application.Dto.Pagination;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace KFS.src.infrastructure.Repository
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly KFSContext _context;
        public FeedbackRepository(KFSContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckUserBoughtProduct(Guid userId, Guid productId)
        {
            var orders = await _context.Orders
                .Where(x => x.UserId == userId && x.Status == OrderStatusEnum.Paid)
                .Include(x => x.OrderItems)
                .ToListAsync();
            var orderItems = _context.OrderItems.AsQueryable();
            foreach (var order in orders)
            {
                foreach (var orderItem in order.OrderItems)
                {
                    if (orderItem is OrderItemProduct orderItemProduct)
                    {
                        if (orderItemProduct.ProductId == productId)
                        {
                            return true;
                        }
                    }
                    if (orderItem is OrderItemBatch orderItemBatch)
                    {
                        var batch = await _context.Batches
                            .Where(x => x.Id == orderItemBatch.BatchId)
                            .Include(x => x.Products)
                            .FirstOrDefaultAsync() ?? throw new Exception("batch null");
                        foreach (var product in batch.Products)
                        {
                            if (product.Id == productId)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public async Task<bool> CreateFeedback(Feedback feedback)
        {
            feedback.CreatedAt = DateTime.Now;
            _context.Feedbacks.Add(feedback);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteFeedback(Guid id)
        {
            var feedback = _context.Feedbacks.FirstOrDefault(x => x.Id == id);
            if (feedback == null) throw new Exception("Feedback not found");
            _context.Feedbacks.Remove(feedback);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Feedback> GetFeedbackById(Guid id)
        {
            return await _context.Feedbacks.FindAsync(id) ?? throw new Exception("Feedback not found");
        }

        public async Task<List<Feedback>> GetFeedbackByUserId(Guid id)
        {
            return await _context.Feedbacks.Where(x => x.UserId == id).ToListAsync();
        }

        public Task<Pagination.ObjectPaging<Feedback>> GetFeedbacks(FeedbackQuery feedbackQuery)
        {
            var query = _context.Feedbacks.AsQueryable();
            // query syntax
            query = query.Where(x => x.ProductId == feedbackQuery.ProductId || feedbackQuery.ProductId == null);
            query = query.Where(x => x.Rating == feedbackQuery.Rating || feedbackQuery.Rating == null);
            // set total
            var total = query.Count();

            // return paging
            var feedbackList = query
            .Skip((feedbackQuery.Page - 1) * feedbackQuery.PageSize)
            .Take(feedbackQuery.PageSize)
            .ToList();
            return Task.FromResult(new Pagination.ObjectPaging<Feedback>
            {
                List = feedbackList,
                Total = total
            });

        }
        // return true if user have feedback on product
        public async Task<bool> HaveUserFeedbackOnProduct(Guid userId, Guid productId)
        {
            return await _context.Feedbacks.Where(x => x.UserId == userId && x.ProductId == productId).CountAsync() != 0;
        }

        public async Task<bool> UpdateFeedback(Feedback feedback)
        {
            DateTime nowVietnam = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
            feedback.UpdatedAt = nowVietnam;
            _context.Feedbacks.Update(feedback);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}