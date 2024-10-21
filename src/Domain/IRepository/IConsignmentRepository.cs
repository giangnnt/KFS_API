using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Domain.Entities;

namespace KFS.src.Domain.IRepository
{
    public interface IConsignmentRepository
    {
        Task<bool> CreateConsignment(Consignment consignment);
        Task<bool> UpdateConsignment(Consignment consignment);
        Task<bool> DeleteConsignment(Consignment consignment);
        Task<Consignment> GetConsignmentById(Guid id);
        Task<IEnumerable<Consignment>> GetConsignments();
        Task<IEnumerable<Consignment>> GetConsignmentsByUserId(Guid userId);
    }
}