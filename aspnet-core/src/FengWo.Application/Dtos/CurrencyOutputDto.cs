using System.Collections.Generic;

namespace FengWo.Dtos
{
    /// <summary>
    /// 通用数据表单Dto
    /// </summary>
    public class CurrencyOutputDto
    {
        /// <summary>
        /// 列表
        /// </summary>
        public List<Dictionary<string,object>> Rows { get; set; }

        /// <summary>
        /// 总行数
        /// </summary>
        public int Total { get; set; }
    }
}
