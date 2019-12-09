using System;
using System.Collections.Generic;
using System.Text;

namespace FengWo.Dtos.Interface
{
    public interface ILimitedResultRequest
    {

        /// <summary>
        /// 每页显示数量
        /// </summary>
        int Limit { get; set; }
    }
}
