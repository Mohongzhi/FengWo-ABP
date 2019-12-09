using FengWo.Authorization;
using FengWo.Authorization.Menus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FengWo.EntityFrameworkCore.Seed.Host
{
    public class DefaultMenusCreator
    {
        private readonly FengWoDbContext _context;

        public static List<Menu> _menus => CreateMenus();

        public DefaultMenusCreator(FengWoDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <returns></returns>
        private static List<Menu> CreateMenus()
        {
            return new List<Menu>()
            {
                new Menu(){ Name = "SystemManage", LocalizationName="SystemManage", Icon="settings",RequiredPermissionName=PermissionNames.Pages_SystemManage, RequiresAuthentication=true,Order=999 },
                new Menu(){ Name = "Users", LocalizationName = "Users",Url="Users",Icon="people", RequiredPermissionName= PermissionNames.Pages_Users, ParentMenuName="SystemManage",RequiresAuthentication=true },
                new Menu(){ Name = "Roles", LocalizationName = "Roles",Url="Roles",Icon="local_offer", RequiredPermissionName= PermissionNames.Pages_Roles, ParentMenuName="SystemManage",RequiresAuthentication=true},
                new Menu(){ Name = "Menus", LocalizationName = "Menus",Url="Menus",Icon="reorder", RequiredPermissionName= PermissionNames.Pages_Menus, ParentMenuName="SystemManage",RequiresAuthentication=true},
                new Menu(){ Name = "Orgnizations", LocalizationName = "Orgnizations",Url="OrgnizationUnit",Icon="group", RequiredPermissionName= PermissionNames.Pages_Orgnizations, ParentMenuName="SystemManage",RequiresAuthentication=true},

            };
        }

        public void Create()
        {
            foreach (var item in _menus)
            {
                if (_context.Menus.FirstOrDefault(p => p.Name == item.Name && p.Url == item.Url) == null)
                {
                    if (item.ParentMenuName != "")
                    {
                        var parent = _context.Menus.FirstOrDefault(p => p.Name == item.ParentMenuName);
                        if (parent != null)
                        {
                            item.ParentMenuId = parent.Id;
                        }
                    }
                    _context.Menus.Add(item);
                }
                _context.SaveChanges();
            }
        }
    }
}
