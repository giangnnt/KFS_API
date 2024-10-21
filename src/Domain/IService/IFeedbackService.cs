using KFS.src.Application.Dto.FeedbackDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Domain.Entities;

namespace KFS.src.Domain.IService
{
    public interface IFeedbackService
    {
        public Task<ResponseDto> GetAll();
        public Task<ResponseDto> GetFeedbackById(Guid id);
        public Task<ResponseDto> Create(FeedbackCreate fed);
        public Task<ResponseDto> Delete(Guid id);
        public Task<ResponseDto> Update(Guid id, FeedbackUpdate fed);

    }
}
