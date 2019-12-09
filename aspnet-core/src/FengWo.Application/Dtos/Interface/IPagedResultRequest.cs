using System;
using System.Collections.Generic;
using System.Text;

namespace FengWo.Dtos.Interface
{
    public interface IPagedResultRequest : ILimitedResultRequest
    {
        /// <summary>
        /// 跳过数量
        /// </summary>
        int Offset { get; set; }
    }
}
