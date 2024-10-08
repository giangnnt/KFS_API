using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.BatchDtos;
using KFS.src.Application.Dto.ResponseDtos;

namespace KFS.src.Domain.IService
{
    public interface IBatchService
    {
        public Task<ResponseDto> GetBatches();
        public Task<ResponseDto> GetBatchById(Guid id);
        public Task<ResponseDto> CreateBatchFromProduct(BatchCreate batch, Guid productId);
        public Task<ResponseDto> UpdateBatch(BatchUpdate batch, Guid id);
        public Task<ResponseDto> DeleteBatch(Guid id);
        
    }
}