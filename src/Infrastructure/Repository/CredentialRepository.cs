using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace KFS.src.Infrastructure.Repository
{
    public class CredentialRepository : ICredentialRepository
    {
        private readonly KFSContext _context;
        public CredentialRepository(KFSContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateCredential(Credential credential)
        {
            _context.Credentials.Add(credential);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteCredential(Guid id)
        {
            _context.Credentials.Remove(_context.Credentials.Find(id) ?? throw new Exception("Credential not found"));
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Credential> GetCredentialById(Guid id)
        {
            return await _context.Credentials.FindAsync(id) ?? throw new Exception("Credential not found");
        }

        public async Task<List<Credential>> GetCredentials()
        {
            return await _context.Credentials.ToListAsync();
        }

        public async Task<List<Credential>> GetCredentialsByProductId(Guid productId)
        {
            return await _context.Credentials.Where(x => x.Product.Id == productId).ToListAsync();
        }

        public async Task<List<Credential>> GetCredentialsByUserOrderHistory(Guid userId)
        {
            var orders = await _context.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Credentials)
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return orders.SelectMany(order => order.OrderItems)
                         .SelectMany(orderItem => orderItem.Product.Credentials)
                         .ToList();
        }

        public async Task<bool> UpdateCredential(Credential credential)
        {
            _context.Credentials.Update(credential);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}