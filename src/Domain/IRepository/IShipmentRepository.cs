using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Domain.Entities;

namespace KFS.src.Domain.IRepository
{
    public interface IShipmentRepository
    {
        Task<IEnumerable<Shipment>> GetShipments();
        Task<Shipment> GetShipmentById(Guid id);
        Task<bool> CreateShipment(Shipment shipment);
        Task<bool> UpdateShipment(Shipment shipment);
        Task<bool> DeleteShipment(Guid id);
    }
}