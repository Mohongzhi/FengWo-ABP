using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using Abp.Domain.Entities;
using FengWo.OrgnizationUnits;
using Abp.Organizations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FengWo
{
    /// <summary>
    /// 组织架构
    /// </summary>
    [AutoMap(typeof(OrganizationUnit))]
    public class OrgnizationUnitDto : Entity<long>, ICreationAudited
    {
        /// <summary>
        /// 代码
        /// </summary>
        [Required]
        [StringLength(95)]
        public string Code{ get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public virtual long? ParentId { get; set; }

        /// <summary>
        /// 父级菜单
        /// </summary>
        [ForeignKey("ParentId")]
        public virtual OrganizationUnit Parent { get; set; }

        //
        // 摘要:
        //     TenantId of this entity.
        public virtual int? TenantId { get; set; }
        //
        // 摘要:
        //     Children of this OU.
        public virtual ICollection<OrganizationUnit> Children { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(128)]
        public string DisplayName { get; set; }

        /// <summary>
        /// 创建者ID
        /// </summary>
        public long? CreatorUserId { get; set; }

        /// <summary>
        /// 创建者时间
        /// </summary>
        public DateTime CreationTime { get; set; }

    }
}
