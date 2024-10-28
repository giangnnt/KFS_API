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
    public class AddressRepository : IAddressRepository
    {
        private readonly KFSContext _context;
        public AddressRepository(KFSContext context)
        {
            _context = context;
        }
        
        public async Task<bool> AddAddressAsync(Address address)
        {
            _context.Addresses.Add(address);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAddressAsync(Guid id)
        {
            _context.Addresses.Remove(_context.Addresses.Find(id) ?? throw new Exception("Address not found"));
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Address> GetAddressById(Guid id)
        {
            return await _context.Addresses.FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Address not found");
        }

        public async Task<IEnumerable<Address>> GetAddressByUserId(Guid userId)
        {
            return await _context.Addresses.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<bool> UpdateAddressAsync(Address address)
        {
            throw new NotImplementedException();
        }
    }
}