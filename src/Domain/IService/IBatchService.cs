using KFS.src.Application.Dto.BatchDtos;
using KFS.src.Application.Dto.ResponseDtos;

namespace KFS.src.Domain.IService
{
    public interface IBatchService
    {
        public Task<ResponseDto> GetBatches();
        public Task<ResponseDto> GetBatchById(Guid id);
        public Task<ResponseDto> CreateBatch(BatchCreate batch);
        public Task<ResponseDto> UpdateBatch(BatchUpdate batch, Guid id);
        public Task<ResponseDto> DeleteBatch(Guid id);
        public Task<ResponseDto> UpdateBatchIsActive(bool isActive, Guid id);
    }
}