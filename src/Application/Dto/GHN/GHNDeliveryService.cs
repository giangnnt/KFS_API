using System.Text.Json.Serialization;

namespace KFS.src.Application.Dto.GHN
{
    public class GHNDeliveryServiceRequest
    {
        [JsonPropertyName("shop_id")]
        public int ShopId { get; set; }
        [JsonPropertyName("from_district")]
        public int FromDistrict { get; set; }
        [JsonPropertyName("to_district")]
        public int ToDistrict { get; set; }
    }
    public class GHNDeliveryServiceResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("message")]
        public string? Message { get; set; }
        [JsonPropertyName("data")]
        public List<ServiceData> Data { get; set; } = new List<ServiceData>();
    }
    public class ServiceData
    {
        [JsonPropertyName("service_id")]
        public int ServiceId { get; set; }
        [JsonPropertyName("short_name")]
        public string ShortName { get; set; } = null!;
        [JsonPropertyName("service_type_id")]
        public int ServiceTypeId { get; set; }
    }
}
