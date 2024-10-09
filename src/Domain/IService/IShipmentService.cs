using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Application.Dto.ShipmentDtos;
using KFS.src.Application.Enum;

namespace KFS.src.Domain.IService
{
    public interface IShipmentService
    {
        Task<ResponseDto> CreateShipment(Guid orderId);
        Task<ResponseDto> UpdateShipment(UpdateDto updateDto, Guid id);
        Task<ResponseDto> DeleteShipment(Guid id);
        Task<ResponseDto> GetShipments();
        Task<ResponseDto> GetShipmentById(Guid id);
        Task<ResponseDto> ShipmentDelivered(Guid id);
    }
}