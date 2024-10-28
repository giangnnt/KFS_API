using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Domain.Entities;

namespace KFS.src.Domain.IRepository
{
    public interface IAddressRepository
    {
        Task<bool> AddAddressAsync(Address address);
        Task<bool> UpdateAddressAsync(Address address);
        Task<Address> GetAddressById(Guid id);
        Task<bool> DeleteAddressAsync(Guid id);
        Task<IEnumerable<Address>> GetAddressByUserId(Guid userId);
    }
}