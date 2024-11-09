using KFS.src.Application.Dto.GHN;

namespace KFS.src.Domain.IService
{
    public interface IGHNService
    {
        public Task<GHNResponse> CalculateShippingFee(GHNRequest request);
    }
}