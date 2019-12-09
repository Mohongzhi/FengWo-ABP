using Abp.AspNetCore.Mvc.ViewComponents;

namespace FengWo.Web.Views
{
    public abstract class FengWoViewComponent : AbpViewComponent
    {
        protected FengWoViewComponent()
        {
            LocalizationSourceName = FengWoConsts.LocalizationSourceName;
        }
    }
}
