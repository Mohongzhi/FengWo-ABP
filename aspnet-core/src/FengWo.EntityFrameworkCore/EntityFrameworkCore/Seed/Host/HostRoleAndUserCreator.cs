using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using FengWo.Authorization;
using FengWo.Authorization.Roles;
using FengWo.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace FengWo.EntityFrameworkCore.Seed.Host
{
    public class HostRoleAndUserCreator
    {
        private readonly FengWoDbContext _context;

        public HostRoleAndUserCreator(FengWoDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateHostRoleAndUsers();
            CreateDefaultRoles();
        }

        private void CreateDefaultRoles()
        {
            List<Role> roles = new List<Role>();
            //创建质监默认角色
            roles.Add(new Role() { IsDefault = false, IsStatic = true, Name = "校领导办公室", NormalizedName = "校领导办公室", DisplayName = "校领导办公室" });
            roles.Add(new Role() { IsDefault = false, IsStatic = true, Name = "办公室", NormalizedName = "办公室", DisplayName = "办公室" });
            roles.Add(new Role() { IsDefault = false, IsStatic = true, Name = "政工处（纪检监察室）", NormalizedName = "政工处（纪检监察室）", DisplayName = "政工处（纪检监察室）" });
            roles.Add(new Role() { IsDefault = false, IsStatic = true, Name = "教务处（竞赛办）", NormalizedName = "教务处（竞赛办）", DisplayName = "教务处（竞赛办）" });
            roles.Add(new Role() { IsDefault = false, IsStatic = true, Name = "学生工作处（安全保卫处）", NormalizedName = "学生工作处（安全保卫处）", DisplayName = "学生工作处（安全保卫处）" });
            roles.Add(new Role() { IsDefault = false, IsStatic = true, Name = "招生就业处", NormalizedName = "招生就业处", DisplayName = "招生就业处" });
            roles.Add(new Role() { IsDefault = false, IsStatic = true, Name = "对外交流与培训处（校企合作办）", NormalizedName = "对外交流与培训处（校企合作办）", DisplayName = "对外交流与培训处（校企合作办）" });
            roles.Add(new Role() { IsDefault = false, IsStatic = true, Name = "设备采购与管理处", NormalizedName = "设备采购与管理处", DisplayName = "设备采购与管理处" });
            roles.Add(new Role() { IsDefault = false, IsStatic = true, Name = "总务处", NormalizedName = "总务处", DisplayName = "总务处" });
            roles.Add(new Role() { IsDefault = false, IsStatic = true, Name = "财务处", NormalizedName = "财务处", DisplayName = "财务处" });
            roles.Add(new Role() { IsDefault = false, IsStatic = true, Name = "人力资源处", NormalizedName = "人力资源处", DisplayName = "人力资源处" });
            roles.Add(new Role() { IsDefault = false, IsStatic = true, Name = "商贸服务产业系", NormalizedName = "商贸服务产业系", DisplayName = "商贸服务产业系" });
            roles.Add(new Role() { IsDefault = false, IsStatic = true, Name = "先进制造产业系", NormalizedName = "先进制造产业系", DisplayName = "先进制造产业系" });
            roles.Add(new Role() { IsDefault = false, IsStatic = true, Name = "新能源应用产业系", NormalizedName = "新能源应用产业系", DisplayName = "新能源应用产业系" });
            roles.Add(new Role() { IsDefault = false, IsStatic = true, Name = "信息服务产业系", NormalizedName = "信息服务产业系", DisplayName = "信息服务产业系" });
            roles.Add(new Role() { IsDefault = false, IsStatic = true, Name = "文化创意产业系", NormalizedName = "文化创意产业系", DisplayName = "文化创意产业系" });
            roles.Add(new Role() { IsDefault = false, IsStatic = true, Name = "通用能力建设中心", NormalizedName = "通用能力建设中心", DisplayName = "通用能力建设中心" });
            roles.Add(new Role() { IsDefault = false, IsStatic = true, Name = "信息中心", NormalizedName = "信息中心", DisplayName = "信息中心" });
            roles.Add(new Role() { IsDefault = false, IsStatic = true, Name = "训练中心（世界技能人才研究中心）", NormalizedName = "训练中心（世界技能人才研究中心）", DisplayName = "训练中心（世界技能人才研究中心）" });
            roles.Add(new Role() { IsDefault = false, IsStatic = true, Name = "校园文化中心", NormalizedName = "校园文化中心", DisplayName = "校园文化中心" });
            roles.Add(new Role() { IsDefault = false, IsStatic = true, Name = "创新创业指导中心", NormalizedName = "创新创业指导中心", DisplayName = "创新创业指导中心" });
            roles.Add(new Role() { IsDefault = false, IsStatic = true, Name = "质量检测中心", NormalizedName = "质量检测中心", DisplayName = "质量检测中心" });
            roles.Add(new Role() { IsDefault = false, IsStatic = true, Name = "团委", NormalizedName = "团委", DisplayName = "团委" });
            roles.Add(new Role() { IsDefault = false, IsStatic = true, Name = "工会", NormalizedName = "工会", DisplayName = "工会" });

            CreateRoleWithNotExists(roles);
        }

        private void CreateRoleWithNotExists(List<Role> list)
        {
            foreach (var item in list)
            {
                if (!_context.Roles.IgnoreQueryFilters().Any(p => p.Name == item.Name))
                {
                    _context.Roles.Add(item);
                    _context.SaveChanges();
                }
            }
        }


        private void CreateHostRoleAndUsers()
        {
            // Admin role for host

            var adminRoleForHost = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == StaticRoleNames.Host.Admin);
            if (adminRoleForHost == null)
            {
                adminRoleForHost = _context.Roles.Add(new Role(null, StaticRoleNames.Host.Admin, StaticRoleNames.Host.Admin) { IsStatic = true, IsDefault = true }).Entity;
                _context.SaveChanges();
            }

            
            // Grant all permissions to admin role for host

            var grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == null && p.RoleId == adminRoleForHost.Id)
                .Select(p => p.Name)
                .ToList();

            var permissions = PermissionFinder
                .GetAllPermissions(new FengWoAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Host) &&
                            !grantedPermissions.Contains(p.Name))
                .ToList();

            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = null,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = adminRoleForHost.Id
                    })
                );
                _context.SaveChanges();
            }

            // Admin user for host

            var adminUserForHost = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == null && u.UserName == AbpUserBase.AdminUserName);
            if (adminUserForHost == null)
            {
                var user = new User
                {
                    TenantId = null,
                    UserName = AbpUserBase.AdminUserName,
                    Name = "admin",
                    Surname = "admin",
                    EmailAddress = "admin@gzittc.com",
                    IsEmailConfirmed = true,
                    IsActive = true
                };

                user.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(user, "2019gzittcZLJC");
                user.SetNormalizedNames();

                adminUserForHost = _context.Users.Add(user).Entity;
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(null, adminUserForHost.Id, adminRoleForHost.Id));
                _context.SaveChanges();

                _context.SaveChanges();
            }
        }
    }
}
