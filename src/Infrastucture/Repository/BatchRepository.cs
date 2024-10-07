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
    public class BatchRpository : IBatchRepository
    {
        private readonly KFSContext _context;
        public BatchRpository(KFSContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateBatch(Batch batch)
        {
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
            return await _context.Batches.ToListAsync();
        }

        public async Task<Batch> GetBatchById(Guid id)
        {
            return await _context.Batches.FindAsync(id) ?? throw new Exception("Batch not found");
        }

        public async Task<bool> UpdateBatch(Batch batch)
        {
            _context.Batches.Update(batch);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}