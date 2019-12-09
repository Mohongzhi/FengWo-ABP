using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using FengWo.Configuration.Dto;

namespace FengWo.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : FengWoAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
