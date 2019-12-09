using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;

namespace FengWo.Web.Views
{
    public abstract class FengWoRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected FengWoRazorPage()
        {
            LocalizationSourceName = FengWoConsts.LocalizationSourceName;
        }
    }
}
