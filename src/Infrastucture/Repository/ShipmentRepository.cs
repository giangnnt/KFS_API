using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.ShipmentDtos;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Infrastucture.Context;
using Microsoft.EntityFrameworkCore;
using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Infrastucture.Repository
{
    public class ShipmentRepository : IShipmentRepository
    {
        private readonly KFSContext _context;
        public ShipmentRepository(KFSContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateShipment(Shipment shipment)
        {
            shipment.CreatedAt = DateTime.Now;
            _context.Shipments.Add(shipment);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteShipment(Guid id)
        {
            var shipment = await _context.Shipments.FirstOrDefaultAsync(x => x.Id == id);
            if (shipment == null) throw new Exception("Shipment not found");
            _context.Shipments.Remove(shipment);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Shipment> GetShipmentById(Guid id)
        {
            return await _context.Shipments
            .Include(x => x.Order)
            .FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Shipment not found");
        }

        public async Task<ObjectPaging<Shipment>> GetShipments(ShipmentQuery shipmentQuery)
        {
            var query = _context.Shipments.AsQueryable();
            // search syntax
            query = query.Where(p => p.Status == shipmentQuery.Status || shipmentQuery.Status == null);
            //set total
            var total = await query.CountAsync();

            // return
            var shipmentList = await query
            .Include(x => x.Order)
            .Skip((shipmentQuery.Page - 1) * shipmentQuery.PageSize)
            .Take(shipmentQuery.PageSize)
            .ToListAsync();
            return new ObjectPaging<Shipment>
            {
                List = shipmentList,
                Total = total
            };
        }

        public async Task<bool> UpdateShipment(Shipment shipment)
        {
            DateTime nowVietnam = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
            shipment.UpdatedAt = nowVietnam;
            _context.Shipments.Update(shipment);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}