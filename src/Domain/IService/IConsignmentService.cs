using KFS.src.Application.Dto.ConsignmentDtos;
using KFS.src.Application.Dto.ResponseDtos;

namespace KFS.src.Domain.IService
{
    public interface IConsignmentService
    {
        Task<ResponseDto> CreateConsignmentOnline(ConsignmentCreateByOrderItem req);
        Task<ResponseDto> CreateConsignment(ConsignmentCreate req);
        Task<ResponseDto> UpdateConsignment(Guid id, ConsignmentUpdate consignmentUpdate);
        Task<ResponseDto> DeleteConsignment(Guid id);
        Task<ResponseDto> GetConsignmentsAdmin(ConsignmentQuery consignmentQuery);
        Task<ResponseDto> GetConsignmentById(Guid id);
        Task<ResponseDto> EvaluateConsignment(bool isApproved, Guid id);
        Task<ResponseDto> PayForConsignment(Guid id);
        public Task<ResponseDto> GetResponsePaymentUrl();
        Task<ResponseDto> GetConsignmentByUserId(ConsignmentQuery consignmentQuery, Guid userId);
        Task<ResponseDto> GetOwnConsignment(ConsignmentQuery consignmentQuery);
    }
}