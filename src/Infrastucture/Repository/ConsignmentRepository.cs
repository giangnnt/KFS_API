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
    public class ConsignmentRepository : IConsignmentRepository
    {
        private readonly KFSContext _context;
        public ConsignmentRepository(KFSContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateConsignment(Consignment consignment)
        {
            consignment.CreatedAt = DateTime.Now;
            _context.Consignments.Add(consignment);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteConsignment(Guid id)
        {
            var consignment = await _context.Consignments.FindAsync(id);
            if (consignment == null) throw new Exception("Consignment not found");
            _context.Consignments.Remove(consignment);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Consignment> GetConsignmentById(Guid id)
        {
            return await _context.Consignments
            .Include(x => x.Product)
            .ThenInclude(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Consignment not found");
        }


        public async Task<IEnumerable<Consignment>> GetConsignments()
        {
            return await _context.Consignments
            .Include(x => x.Product)
            .ThenInclude(x => x.Category)
            .ToListAsync();
        }

        public async Task<bool> UpdateConsignment(Consignment consignment)
        {
            _context.Consignments.Update(consignment);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}