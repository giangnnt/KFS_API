using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace KFS.src.Infrastructure.Repository
{
    public class BatchRpository : IBatchRepository
    {
        private readonly KFSContext _context;
        public BatchRpository(KFSContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateBatch(Batch batch)
        {
            batch.CreatedAt = DateTime.Now;
            _context.Batches.Add(batch);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteBatch(Guid id)
        {
            _context.Batches.Remove(_context.Batches.Find(id) ?? throw new Exception("Batch not found"));
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<List<Batch>> GetAllBatches()
        {
            return await _context.Batches
            .Include(x => x.Promotions)
            .Include(x => x.Products)
            .ToListAsync();
        }

        public async Task<Batch> GetBatchById(Guid id)
        {
            return await _context.Batches
            .Include(x => x.Promotions)
            .Include(x => x.Products)
            .FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Batch not found");
        }

        public async Task<bool> UpdateBatch(Batch batch)
        {
            DateTime nowVietnam = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
            batch.UpdatedAt = nowVietnam;
            _context.Batches.Update(batch);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}