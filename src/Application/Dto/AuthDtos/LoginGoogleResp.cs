namespace KFS.src.Application.Dto.AuthDtos
{
    public class LoginGoogleResp
    {
        public string AccessToken { get; set; } = null!;
        public long AccessTokenExp { get; set; }
        public string AccessTokenType { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public string Scope { get; set; } = null!;
    }
}
