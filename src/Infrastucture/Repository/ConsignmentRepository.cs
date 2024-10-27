using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.ConsignmentDtos;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Infrastucture.Context;
using Microsoft.EntityFrameworkCore;
using static KFS.src.Application.Dto.Pagination.Pagination;

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

        public async Task<bool> DeleteConsignment(Consignment consignment)
        {
            _context.Consignments.Remove(consignment);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Consignment> GetConsignmentById(Guid id)
        {
            return await _context.Consignments
            .Include(x => x.Product)
            .ThenInclude(x => x.Batches)
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Consignment not found");
        }


        public async Task<ObjectPaging<Consignment>> GetConsignmentsAdmin(ConsignmentQuery consignmentQuery)
        {
            var query = _context.Consignments.AsQueryable();
            // search syntax
            query = query.Where(c => c.Status == consignmentQuery.Status || consignmentQuery.Status == null);
            query = query.Where(c => c.IsForSell == consignmentQuery.IsForSell || consignmentQuery.IsForSell == null);
            query = query.Where(c => c.ExpiryDate <= consignmentQuery.ExpiryDateBefore || consignmentQuery.ExpiryDateBefore == null);
            // set total
            var total = await query.CountAsync();
            // return

            var consignmentList = await query
            .Include(x => x.Product)
            .ThenInclude(x => x.Batches)
            .Include(x => x.User)
            .Skip((consignmentQuery.Page - 1) * consignmentQuery.PageSize)
            .Take(consignmentQuery.PageSize)
            .ToListAsync();
            return new ObjectPaging<Consignment>
            {
                List = consignmentList,
                Total = total
            };
        }

        public async Task<ObjectPaging<Consignment>> GetConsignmentsByUserId(ConsignmentQuery consignmentQuery, Guid userId)
        {
            var query = _context.Consignments.AsQueryable();
            // search syntax
            query = query.Where(c => c.Status == consignmentQuery.Status || consignmentQuery.Status == null);
            query = query.Where(c => c.IsForSell == consignmentQuery.IsForSell || consignmentQuery.IsForSell == null);
            query = query.Where(c => c.ExpiryDate <= consignmentQuery.ExpiryDateBefore || consignmentQuery.ExpiryDateBefore == null);
            query = query.Where(c => c.UserId == userId);
            // set total
            var total = await query.CountAsync();
            // return
            var consignmentList = await query
            .Include(x => x.Product)
            .ThenInclude(x => x.Batches)
            .Include(x => x.User)
            .ToListAsync();
            return new ObjectPaging<Consignment>
            {
                List = consignmentList,
                Total = total
            };
        }

        public async Task<IEnumerable<Consignment>> GetConsignmentsByUserId(Guid userId)
        {
            return await _context.Consignments
            .Include(x => x.Product)
            .ThenInclude(x => x.Batches)
            .Include(x => x.User)
            .Where(x => x.UserId == userId)
            .ToListAsync();
        }

        public async Task<bool> UpdateConsignment(Consignment consignment)
        {
            DateTime nowVietnam = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
            consignment.UpdatedAt = nowVietnam;
            _context.Consignments.Update(consignment);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}