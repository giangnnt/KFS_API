using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using KFS.src.Application.Dto.ResponseDtos;

namespace KFS.src.Domain.IService
{
    public interface IPaymentService
    {
        Task<ResponseDto> GetPaymentById(Guid id);
        Task<ResponseDto> GetPayments();
        Task<ResponseDto> CreatePayment();
        Task<ResponseDto> UpdatePayment();
        Task<ResponseDto> DeletePayment(Guid id);
        Task<ResponseDto> CreatePaymentByOrderIdCOD(Guid orderId);
        Task<ResponseDto> GetPaymentByUser(Guid userId);
    }
}