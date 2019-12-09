using System.ComponentModel.DataAnnotations;

namespace FengWo.Users.Dto
{
    /// <summary>
    /// 更改密码Dto
    /// </summary>
    public class ChangePasswordDto
    {
        /// <summary>
        /// 当前密码
        /// </summary>
        [Required(ErrorMessage ="当前密码为必填项")]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required(ErrorMessage = "新密码为必填项")]
        public string NewPassword { get; set; }
    }
}
