using KFS.src.Application.Dto.ShipmentDtos;
using KFS.src.Domain.Entities;
using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Domain.IRepository
{
    public interface IShipmentRepository
    {
        Task<ObjectPaging<Shipment>> GetShipments(ShipmentQuery shipmentQuery);
        Task<Shipment> GetShipmentById(Guid id);
        Task<bool> CreateShipment(Shipment shipment);
        Task<bool> UpdateShipment(Shipment shipment);
        Task<bool> DeleteShipment(Guid id);
    }
}