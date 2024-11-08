using KFS.src.Application.Constant;
using System.ComponentModel.DataAnnotations;

namespace KFS.src.Application.Dto.UserDtos
{
    public class UserCreate
    {

        [Required]
        [StringLength(30)]
        public string FullName { get; set; } = null!;
        [Required]
        [StringLength(30)]
        [RegularExpression(RegexConst.EMAIL, ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = null!;
        [Required]
        [StringLength(15, ErrorMessage = "Phone number must be 0-15 characters")]
        [RegularExpression(RegexConst.PHONE_NUMBER, ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; } = null!;
        public int RoleId { get; set; }
    }
}