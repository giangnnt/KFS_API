using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Constant;

namespace KFS.src.Application.Dto.UserDtos
{
    public class ChangePasswordDto
    {
        public string OldPassword { get; set; } = null!;
        [Required]
        [StringLength(20, ErrorMessage = "Password must be at least 6 characters", MinimumLength = 6)]
        [RegularExpression(RegexConst.PASSWORD, ErrorMessage = "Password must contain at least 1 uppercase letter, 1 lowercase letter, 1 special character and 1 number")]
        public string NewPassword { get; set; } = null!;
    }
}