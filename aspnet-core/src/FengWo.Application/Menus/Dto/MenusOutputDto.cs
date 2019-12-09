using FengWo.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FengWo.Menus.Dto
{
    /// <summary>
    /// 获取菜单
    /// </summary>
    public class MenusOutputDto
    {
        /// <summary>
        /// 菜单列表
        /// </summary>
        public List<MenuDto> Rows { get; set; }

        /// <summary>
        /// 总行数
        /// </summary>
        public int Total { get; set; }
    }
}
