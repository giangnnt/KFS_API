using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.VNPay;

namespace KFS.src.Domain.IService
{
    public interface IVNPayService
    {
        public string CreatePaymentUrl(HttpContext context, VNPayRequestModel request);
        VNPayResponseModel GetResponse(IQueryCollection colelction);
    }
}