using System.Text.Json.Serialization;

namespace KFS.src.Application.Dto.GHN
{
    public class GHNDistrictResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("message")]
        public string? Message { get; set; }
        [JsonPropertyName("data")]
        public List<DistrictData> Data { get; set; } = new List<DistrictData>();
    }

    public class DistrictData
    {
        [JsonPropertyName("DistrictID")]
        public int DistrictID { get; set; }
        [JsonPropertyName("DistrictName")]
        public string DistrictName { get; set; } = null!;
        [JsonPropertyName("ProvinceID")]
        public int ProvinceID { get; set; }
        [JsonPropertyName("NameExtension")]
        public List<string> NameExtension { get; set; } = new List<string>();
    }
}
