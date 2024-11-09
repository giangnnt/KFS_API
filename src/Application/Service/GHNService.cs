using KFS.src.Application.Dto.GHN;
using KFS.src.Domain.IService;
using System.Text.Json;

namespace KFS.src.Application.Service
{
    public class GHNService : IGHNService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public GHNService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            _httpClient.DefaultRequestHeaders.Add("token", _configuration["GHN:Token"]);
            _httpClient.DefaultRequestHeaders.Add("shop_id", _configuration["GHN:ShopId"]);
            _httpClient.BaseAddress = new Uri(_configuration["GHN:BaseUrl"]);
        }
        public async Task<GHNResponse> CalculateShippingFee(GHNRequest request)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Patch, new Uri(_httpClient.BaseAddress, "/shiip/public-api/v2/shipping-order/fee"))
            {
                Content = JsonContent.Create(request)
            };

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {

                var jsonRespone = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<GHNResponse>(jsonRespone);
                if (result == null)
                {
                    throw new InvalidOperationException("Deserialization returned null.");
                }
                return result;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Error when calling GHN API: {errorContent}");
            }
        }
    }
}