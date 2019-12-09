using System.ComponentModel.DataAnnotations;

namespace FengWo.Users.Dto
{
    /// <summary>
    /// 重置密码Dto
    /// </summary>
    public class ResetPasswordDto
    {
        /// <summary>
        /// 管理员密码
        /// </summary>
        [Required]
        public string AdminPassword { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
        public long UserId { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        public string NewPassword { get; set; }
    }
}
