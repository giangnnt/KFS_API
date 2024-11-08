using KFS.src.Domain.Entities;

namespace KFS.src.Domain.IRepository
{
    public interface IBatchRepository
    {
        public Task<bool> CreateBatch(Batch batch);
        public Task<bool> UpdateBatch(Batch batch);
        public Task<bool> DeleteBatch(Guid id);
        public Task<Batch> GetBatchById(Guid id);
        public Task<List<Batch>> GetAllBatches();
    }
}