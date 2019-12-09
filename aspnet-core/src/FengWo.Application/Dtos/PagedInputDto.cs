﻿using FengWo.Dtos.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace FengWo.Dtos
{
    /// <summary>
    /// 分页
    /// </summary>
    public class PagedInputDto : IPagedResultRequest
    {
        /// <summary>
        /// 跳过数量
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// 每页显示数量
        /// </summary>
        public int Limit { get; set; }
    }
}