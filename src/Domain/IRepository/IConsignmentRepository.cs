using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.ConsignmentDtos;
using KFS.src.Domain.Entities;
using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Domain.IRepository
{
    public interface IConsignmentRepository
    {
        Task<bool> CreateConsignment(Consignment consignment);
        Task<bool> UpdateConsignment(Consignment consignment);
        Task<bool> DeleteConsignment(Consignment consignment);
        Task<Consignment> GetConsignmentById(Guid id);
        Task<ObjectPaging<Consignment>> GetConsignmentsAdmin(ConsignmentQuery consignmentQuery);
        Task<ObjectPaging<Consignment>> GetConsignmentsByUserId(ConsignmentQuery consignmentQuery, Guid userId);
    }
}