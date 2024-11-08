using System.Text.Json.Serialization;

namespace KFS.src.Application.Dto.GHN
{
    public class GHNRequest
    {
        [JsonPropertyName("to_ward_code")]
        public string ToWardCode { get; set; } = null!;
        [JsonPropertyName("to_district_id")]
        public int ToDistrictId { get; set; }
        [JsonPropertyName("weight")]
        public int Weight { get; set; }
        [JsonPropertyName("service_id")]
        public int ServiceId { get; set; }
        [JsonPropertyName("service_type_id")]
        public int ServiceTypeId { get; set; }
    }
}