using KFS.src.Application.Dto.VNPay;

namespace KFS.src.Domain.IService
{
    public interface IVNPayService
    {
        public string CreatePaymentUrl(HttpContext context, VNPayRequestModel request, string UrlPartern);
        VNPayResponseModel GetResponse(IQueryCollection colelction);
    }
}