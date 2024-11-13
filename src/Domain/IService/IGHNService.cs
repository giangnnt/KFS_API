using KFS.src.Application.Dto.GHN;

namespace KFS.src.Domain.IService
{
    public interface IGHNService
    {
        public Task<GHNResponse> CalculateShippingFee(GHNRequest request);
        public Task<GHNProvinceResponse> GetProvinces();
        public Task<GHNDistrictResponse> GetDistricts(int provinceId);
        public Task<GHNWardResponse> GetWards(int districtId);
        public Task<GHNDeliveryServiceResponse> GetServices(GHNDeliveryServiceRequest request);
    }
}