using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Application.Dto.ShipmentDtos;

namespace KFS.src.Domain.IService
{
    public interface IShipmentService
    {
        Task<ResponseDto> CreateShipment(Guid orderId, Guid shipperId);
        Task<ResponseDto> UpdateShipment(UpdateDto updateDto, Guid id);
        Task<ResponseDto> DeleteShipment(Guid id);
        Task<ResponseDto> GetShipments(ShipmentQuery shipmentQuery);
        Task<ResponseDto> GetShipmentById(Guid id);
        Task<ResponseDto> ShipmentDelivered(Guid id, bool IsSuccess);
        Task<ResponseDto> ShipmentCompleted(Guid id, bool IsSuccess);
        Task<ResponseDto> GetShipmentsByShipperId(Guid id);
    }
}