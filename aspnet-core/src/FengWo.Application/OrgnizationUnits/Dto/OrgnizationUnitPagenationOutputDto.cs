using System.Collections.Generic;
namespace FengWo.OrgnizationUnits
{
    /// <summary>
    /// 组织架构分页返回Dto
    /// </summary>
    public class OrgnizationUnitPagenationOutputDto
    {
        /// <summary>
        /// 组织架构数据列表
        /// </summary>
        public List<OrgnizationUnitDto> Rows { get; set; }

        /// <summary>
        /// 总行数
        /// </summary>
        public int Total { get; set; }
    }
}
