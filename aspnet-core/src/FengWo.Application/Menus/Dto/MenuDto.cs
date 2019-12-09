using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using FengWo.Authorization.Menus;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FengWo.Menus.Dto
{
    /// <summary>
    /// 菜单数据传输对象
    /// </summary>
    [AutoMap(typeof(Menu))]
    public class MenuDto:EntityDto,ICreationAudited
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 父菜单ID
        /// </summary>
        public int ParentMenuId { get; set; }

        /// <summary>
        /// 父菜单名称
        /// </summary>
        public string ParentMenuName { get; set; }

        /// <summary>
        /// 本地化名称
        /// </summary>
        [Required]
        public string LocalizationName { get; set; }

        /// <summary>
        /// URL
        /// </summary>
        public string Url { get; set; } = "";

        /// <summary>
        /// Icon
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 是否需要验证身份
        /// </summary>
        public bool RequiresAuthentication { get; set; }

        /// <summary>
        /// 所需权限名
        /// </summary>
        public string RequiredPermissionName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; } = 0;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// 创建人
        /// </summary>
        public long? CreatorUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
