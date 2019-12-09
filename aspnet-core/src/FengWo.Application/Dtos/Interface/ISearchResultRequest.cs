using System;
using System.Collections.Generic;
using System.Text;

namespace FengWo.Dtos.Interface
{
    public interface ISearchResultRequest
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        string Search { get; set; }
    }
}
