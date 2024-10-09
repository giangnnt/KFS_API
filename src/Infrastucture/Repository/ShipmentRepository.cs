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
    public class ShipmentRepository : IShipmentRepository
    {
        private readonly KFSContext _context;
        public ShipmentRepository(KFSContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateShipment(Shipment shipment)
        {
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

        public async Task<IEnumerable<Shipment>> GetShipments()
        {
            return await _context.Shipments
            .Include(x => x.Order)
            .ToListAsync();
        }

        public async Task<bool> UpdateShipment(Shipment shipment)
        {
            _context.Shipments.Update(shipment);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}