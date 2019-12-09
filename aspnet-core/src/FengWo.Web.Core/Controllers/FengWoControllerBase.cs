using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace FengWo.Controllers
{
    public abstract class FengWoControllerBase: AbpController
    {
        protected FengWoControllerBase()
        {
            LocalizationSourceName = FengWoConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
