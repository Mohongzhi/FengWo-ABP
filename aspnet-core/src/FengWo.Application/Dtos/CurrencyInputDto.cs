using FengWo.Dtos;
using System.Collections.Generic;

namespace FengWo.Dtos
{
    /// <summary>
    /// 通用InputDto
    /// </summary>
    public class CurrencyInputDto : PagedSortedAndSearchInputDto
    {
        /// <summary>
        /// 参数
        /// </summary>
        public Dictionary<string, string> Params { get; set; }
    }
}
