using System;
using System.Collections.Generic;
using System.Text;

namespace FengWo.Dtos.Interface
{
    public interface ISortedResultRequest
    {
        /// <summary>
        /// 排序名称
        /// </summary>
        string Sort { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        string Order { get; set; }
    }
}
