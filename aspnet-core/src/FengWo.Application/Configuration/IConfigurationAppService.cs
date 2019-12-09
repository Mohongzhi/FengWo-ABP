using System.Threading.Tasks;
using FengWo.Configuration.Dto;

namespace FengWo.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
