using System.Text.Json.Serialization;

namespace KFS.src.Application.Dto.GHN
{
    public class GHNWardResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("message")]
        public string? Message { get; set; }
        [JsonPropertyName("data")]
        public List<WardData> Data { get; set; } = new List<WardData>();
    }
    public class WardData
    {
        [JsonPropertyName("WardCode")]
        public string WardCode { get; set; } = null!;
        [JsonPropertyName("WardName")]
        public string WardName { get; set; } = null!;
        [JsonPropertyName("NameExtension")]
        public List<string> NameExtension { get; set; } = new List<string>();
    }
}
