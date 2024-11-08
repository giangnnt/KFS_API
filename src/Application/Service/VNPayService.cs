using KFS.src.Application.Dto.VNPay;
using KFS.src.Application.Helper.VNPay;
using KFS.src.Domain.IService;

namespace KFS.src.Application.Service
{
    public class VNPayService : IVNPayService
    {
        private readonly IConfiguration _config;
        public VNPayService(IConfiguration config)
        {
            _config = config;
        }
        public string CreatePaymentUrl(HttpContext context, VNPayRequestModel request, string UrlPartern)
        {
            var tick = DateTime.Now.Ticks.ToString();
            var vnpay = new VNPayLibrary();
            vnpay.AddRequestData("vnp_Version", _config["VNPay:Version"]);
            vnpay.AddRequestData("vnp_Command", _config["VNPay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _config["VNPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", (request.Amount * 100).ToString());
            vnpay.AddRequestData("vnp_CreateDate", request.CreateDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _config["VNPay:CurrCode"]);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", _config["VNPay:Locale"]);
            vnpay.AddRequestData("vnp_OrderInfo", "Payment for order" + request.OrderId.ToString());
            vnpay.AddRequestData("vnp_OrderType", "billpayment");
            if (UrlPartern == "order")
            {
                vnpay.AddRequestData("vnp_ReturnUrl", _config["VNPay:PaymentBackReturnUrlOrder"]);
            }
            if (UrlPartern == "consignment")
            {
                vnpay.AddRequestData("vnp_ReturnUrl", _config["VNPay:PaymentBackReturnUrlConsignment"]);
            }
            vnpay.AddRequestData("vnp_TxnRef", request.OrderId.ToString());
            var paymentUrl = vnpay.CreateRequestUrl(_config["VNPay:BaseUrl"], _config["VNPay:HashSecret"]);
            return paymentUrl;
        }

        public VNPayResponseModel GetResponse(IQueryCollection colelction)
        {
            var vnpay = new VNPayLibrary();
            foreach (var key in colelction.Keys)
            {
                if (!string.IsNullOrEmpty(colelction[key]) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, colelction[key]);
                }
            }
            var orderId = vnpay.GetResponseData("vnp_TxnRef");
            var vnp_Amount = vnpay.GetResponseData("vnp_Amount");
            var vnpTranId = vnpay.GetResponseData("vnp_TransactionNo");
            var vnpResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
            var vnp_SecureHash = vnpay.GetResponseData("vnp_SecureHash");
            var checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["VNPay:HashSecret"]);
            if (checkSignature)
            {
                return new VNPayResponseModel
                {
                    PaymentId = vnpTranId,
                    Success = vnpResponseCode == "00",
                    TransactionStatus = vnp_TransactionStatus,
                    OrderId = orderId,
                    Amount = vnp_Amount,
                    PaymentMethod = "VNPay",
                    OrderDesription = "Payment for order " + orderId,
                    Token = vnp_SecureHash,
                    VnPayResponseCode = vnpResponseCode
                };
            }
            else
            {
                return new VNPayResponseModel
                {
                    PaymentId = vnpTranId,
                    Success = vnpResponseCode == "00",
                    TransactionStatus = vnp_TransactionStatus,
                    OrderId = orderId,
                    Amount = vnp_Amount,
                    PaymentMethod = "VNPay",
                    OrderDesription = "Payment for order " + orderId,
                    Token = vnp_SecureHash,
                    VnPayResponseCode = vnpResponseCode
                };
            }
        }

    }
}
