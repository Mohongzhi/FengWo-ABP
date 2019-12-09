using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace FengWo.Users.Dto
{
    /// <summary>
    /// 基础用户Dto
    /// </summary>
    public class BaseUserDto : EntityDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

    }
}
