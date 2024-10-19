using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.ConsignmentDtos;
using KFS.src.Application.Dto.ResponseDtos;

namespace KFS.src.Domain.IService
{
    public interface IConsignmentService
    {
        Task<ResponseDto> CreateConsignmentOnline(ConsignmentCreateByOrderItem req);
        Task<ResponseDto> CreateConsignment(ConsignmentCreate req);
        Task<ResponseDto> UpdateConsignment(Guid id);
        Task<ResponseDto> DeleteConsignment(Guid id);
        Task<ResponseDto> GetConsignments();
        Task<ResponseDto> GetConsignmentById(Guid id);
        Task<ResponseDto> EvaluateConsignment(bool isApproved, Guid id);
        Task<ResponseDto> PayForConsignment(Guid id);
        Task<ResponseDto> GetResponsePaymentUrl();
        Task<ResponseDto> GetConsignmentByUserId(Guid userId);
    }
}