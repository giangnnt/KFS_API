using KFS.src.Application.Dto.FeedbackDtos;
using KFS.src.Domain.Entities;
using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Domain.IRepository
{
    public interface IFeedbackRepository
    {
        Task<ObjectPaging<Feedback>> GetFeedbacks(FeedbackQuery feedbackQuery);
        Task<bool> CreateFeedback(Feedback feedback);
        Task<bool> UpdateFeedback(Feedback feedback);
        Task<bool> DeleteFeedback(Guid id);
        Task<Feedback> GetFeedbackById(Guid id);
        Task<List<Feedback>> GetFeedbackByUserId(Guid id);
        Task<bool> CheckUserBoughtProduct(Guid userId, Guid productId);
        Task<bool> HaveUserFeedbackOnProduct(Guid userId, Guid productId);
    }
}