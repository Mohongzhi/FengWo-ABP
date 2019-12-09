using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace FengWo.Authorization
{
    /// <summary>
    /// 权限管理
    /// </summary>
    public class FengWoAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Menus, L("Menus"));
            context.CreatePermission(PermissionNames.Pages_SystemManage, L("SystemManage"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            context.CreatePermission(PermissionNames.Pages_Orgnizations, L("Orgnizations"));

            context.CreatePermission(PermissionNames.Pages_Administration_HangfireDashboard, L("HangfireDashboard"));


        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, FengWoConsts.LocalizationSourceName);
        }
    }
}
