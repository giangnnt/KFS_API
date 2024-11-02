using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.FeedbackDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Domain.Entities;

namespace KFS.src.Domain.IService
{
    public interface IFeedbackService
    {
        Task<ResponseDto> GetFeedbacks(FeedbackQuery feedbackQuery);
        Task<ResponseDto> GetAverageRating(Guid productId);
        Task<ResponseDto> CreateFeedback(Guid id, FeedbackCreate req);
        Task<ResponseDto> UpdateFeedback(Guid id, FeedbackUpdate req);
        Task<ResponseDto> DeleteFeedback(Guid id);
    }
}