using System.Text.Json.Serialization;

namespace KFS.src.Application.Dto.GHN
{
    public class GHNProvinceResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("message")]
        public string? Message { get; set; }
        [JsonPropertyName("data")]
        public List<ProvinceData> Data { get; set; } = new List<ProvinceData>();
    }
    public class ProvinceData
    {
        [JsonPropertyName("ProvinceID")]
        public int ProvinceID { get; set; }
        [JsonPropertyName("ProvinceName")]
        public string ProvinceName { get; set; } = null!;
        [JsonPropertyName("Code")]
        public string Code { get; set; } = null!;
        [JsonPropertyName("NameExtension")]
        public List<string> NameExtension { get; set; } = new List<string>();
    }
}
