using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Infrastucture.Context;
using Microsoft.EntityFrameworkCore;

namespace KFS.src.Infrastucture.Repository
{
    public class CredentialRepository : ICredentialRepositoty
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

        public async Task<bool> UpdateCredential(Credential credential)
        {
            _context.Credentials.Update(credential);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}