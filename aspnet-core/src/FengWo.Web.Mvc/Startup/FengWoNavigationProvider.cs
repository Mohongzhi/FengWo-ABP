using Abp.Application.Navigation;
using Abp.Domain.Repositories;
using Abp.Localization;
using FengWo.Authorization;
using FengWo.Authorization.Menus;
using System.Linq;

namespace FengWo.Web.Startup
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public class FengWoNavigationProvider : NavigationProvider
    {
        private readonly IRepository<Menu> _menuRepository;

        public FengWoNavigationProvider(IRepository<Menu> menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public override void SetNavigation(INavigationProviderContext context)
        {
            #region Standard
            //context.Manager.MainMenu
            //        .AddItem(
            //            new MenuItemDefinition(
            //                PageNames.Home,
            //                L("HomePage"),
            //                url: "",
            //                icon: "home",
            //                requiresAuthentication: true
            //            ))
            //        .AddItem(new MenuItemDefinition("SystemMenus", L("SystemMenus"), icon: "home", requiredPermissionName: PermissionNames.Pages_SystemMenus)
            //            .AddItem(new MenuItemDefinition(PageNames.Users, L("Users"), url: "Users", icon: "people", requiredPermissionName: PermissionNames.Pages_Users))
            //            .AddItem(new MenuItemDefinition(PageNames.Roles, L("Roles"), url: "Roles", icon: "local_offer", requiredPermissionName: PermissionNames.Pages_Roles))
            //        )
            //        .AddItem(new MenuItemDefinition(PageNames.Tenants, L("Tenants"), url: "Tenants", icon: "business", requiredPermissionName: PermissionNames.Pages_Tenants))
            //        .AddItem(
            //            new MenuItemDefinition(
            //                PageNames.About,
            //                L("About"),
            //                url: "About",
            //                icon: "info"
            //            ))
            //        .AddItem( // Menu items below is just for demonstration!
            //            new MenuItemDefinition("MultiLevelMenu", L("MultiLevelMenu"), icon: "menu")
            //            .AddItem(
            //                new MenuItemDefinition(
            //                    "AspNetBoilerplate",
            //                    new FixedLocalizableString("ASP.NET Boilerplate")
            //                ).AddItem(
            //                    new MenuItemDefinition(
            //                        "AspNetBoilerplateHome",
            //                        new FixedLocalizableString("Home"),
            //                        url: "https://aspnetboilerplate.com?ref=abptmpl"
            //                    )
            //                ).AddItem(
            //                    new MenuItemDefinition(
            //                        "AspNetBoilerplateTemplates",
            //                        new FixedLocalizableString("Templates"),
            //                        url: "https://aspnetboilerplate.com/Templates?ref=abptmpl"
            //                    )
            //                ).AddItem(
            //                    new MenuItemDefinition(
            //                        "AspNetBoilerplateSamples",
            //                        new FixedLocalizableString("Samples"),
            //                        url: "https://aspnetboilerplate.com/Samples?ref=abptmpl"
            //                    )
            //                ).AddItem(
            //                    new MenuItemDefinition(
            //                        "AspNetBoilerplateDocuments",
            //                        new FixedLocalizableString("Documents"),
            //                        url: "https://aspnetboilerplate.com/Pages/Documents?ref=abptmpl"
            //                    )
            //                )
            //            ).AddItem(
            //                new MenuItemDefinition(
            //                    "AspNetZero",
            //                    new FixedLocalizableString("ASP.NET Zero")
            //                ).AddItem(
            //                    new MenuItemDefinition(
            //                        "AspNetZeroHome",
            //                        new FixedLocalizableString("Home"),
            //                        url: "https://aspnetzero.com?ref=abptmpl"
            //                    )
            //                ).AddItem(
            //                    new MenuItemDefinition(
            //                        "AspNetZeroDescription",
            //                        new FixedLocalizableString("Description"),
            //                        url: "https://aspnetzero.com/?ref=abptmpl#description"
            //                    )
            //                ).AddItem(
            //                    new MenuItemDefinition(
            //                        "AspNetZeroFeatures",
            //                        new FixedLocalizableString("Features"),
            //                        url: "https://aspnetzero.com/?ref=abptmpl#features"
            //                    )
            //                ).AddItem(
            //                    new MenuItemDefinition(
            //                        "AspNetZeroPricing",
            //                        new FixedLocalizableString("Pricing"),
            //                        url: "https://aspnetzero.com/?ref=abptmpl#pricing"
            //                    )
            //                ).AddItem(
            //                    new MenuItemDefinition(
            //                        "AspNetZeroFaq",
            //                        new FixedLocalizableString("Faq"),
            //                        url: "https://aspnetzero.com/Faq?ref=abptmpl"
            //                    )
            //                ).AddItem(
            //                    new MenuItemDefinition(
            //                        "AspNetZeroDocuments",
            //                        new FixedLocalizableString("Documents"),
            //                        url: "https://aspnetzero.com/Documents?ref=abptmpl"
            //                    )
            //                )
            //            )
            //        ); 
            #endregion

            context.Manager.MainMenu
                    .AddItem(new MenuItemDefinition(
                            PageNames.Home,
                            L("HomePage"),
                            url: "",
                            icon: "home",
                            requiresAuthentication: true));

            GetMenus(context.Manager.MainMenu);

        }

        private void GetMenus(MenuDefinition mainMenu)
        {
            var parentMenus = _menuRepository.GetAllList(p => p.ParentMenuId == 0 && p.IsEnabled == true && p.IsVisible == true).OrderBy(p => p.Order);
            foreach (var item in parentMenus)
            {
                mainMenu.AddItem(new MenuItemDefinition(item.Name, L(item.LocalizationName), icon: item.Icon, url: item.Url,
                    requiresAuthentication: item.RequiresAuthentication, requiredPermissionName: item.RequiredPermissionName, order: item.Order,
                    isEnabled: item.IsEnabled, isVisible: item.IsVisible));
                var menuItem = mainMenu.Items.Find(p => p.Name == item.Name);
                GetChildMenus(menuItem);
            }

        }

        private void GetChildMenus(MenuItemDefinition menuItem)
        {
            var childs = _menuRepository.GetAllList(p => p.ParentMenuName == menuItem.Name && p.IsEnabled == true && p.IsVisible == true && p.Name != menuItem.Name).OrderBy(p => p.Order);
            foreach (var item in childs)
            {
                menuItem.AddItem(new MenuItemDefinition(item.Name, L(item.LocalizationName), icon: item.Icon, url: item.Url,
                    requiresAuthentication: item.RequiresAuthentication, requiredPermissionName: item.RequiredPermissionName, order: item.Order,
                    isEnabled: item.IsEnabled, isVisible: item.IsVisible));
                var currentItem = menuItem.Items.Find(p => p.Name == item.Name);
                GetChildMenus(currentItem);
            }
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, FengWoConsts.LocalizationSourceName);
        }
    }
}
