using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using FengWo.Authorization.Roles;
using FengWo.Authorization.Users;
using FengWo.MultiTenancy;
using FengWo.Authorization.Menus;
using FengWo.Messages;

namespace FengWo.EntityFrameworkCore
{
    public class FengWoDbContext : AbpZeroDbContext<Tenant, Role, User, FengWoDbContext>
    {
        /* Define a DbSet for each entity of the application */

        #region 系统 System
        /// <summary>
        /// 菜单
        /// </summary>
        public DbSet<Menu> Menus { get; set; }

        /// <summary>
        /// 消息通知
        /// </summary>
        public DbSet<Message> Messages { get; set; }

        #endregion

        public FengWoDbContext(DbContextOptions<FengWoDbContext> options)
            : base(options)
        {
        }
    }
}
