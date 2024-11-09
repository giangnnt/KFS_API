using KFS.src.Application.Constant;
using System.ComponentModel.DataAnnotations;

namespace KFS.src.Application.Dto.AuthDtos
{
    public class LoginDto
    {
        [Required]
        [StringLength(30)]
        [RegularExpression(RegexConst.EMAIL, ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
    public class TokenResp
    {
        public string AccessToken { get; set; } = null!;
        public long AccessTokenExp { get; set; }
        public string RefreshToken { get; set; } = null!;
        public long RefreshTokenExp { get; set; }
    }
}