using System.Collections.Generic;
namespace FengWo.Users.Dto
{
    /// <summary>
    /// 用户分页返回Dto
    /// </summary>
    public class UserPagenationOutputDto
    {
        /// <summary>
        /// 测试数据列表
        /// </summary>
        public List<UserDto> Rows { get; set; }

        /// <summary>
        /// 总行数
        /// </summary>
        public int Total { get; set; }
    }
}
