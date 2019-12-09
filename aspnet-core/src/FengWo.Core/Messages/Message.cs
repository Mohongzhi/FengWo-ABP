using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace FengWo.Messages
{
    /// <summary>
    /// 消息通知
    /// </summary>
    public class Message : Entity, IFullAudited
    {
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 动作类型
        /// </summary>
        public string ActionType { get; set; }

        /// <summary>
        /// 跳转链接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 接收者Id
        /// </summary>
        public long ReceiveUserId { get; set; }

        /// <summary>
        /// 发送者Id
        /// </summary>
        public long? SendUserId { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public long? CreatorUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 最后修改者
        /// </summary>
        public long? LastModifierUserId { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// 删除者
        /// </summary>
        public long? DeleterUserId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeletionTime { get; set; }

        /// <summary>
        /// 是否已删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
