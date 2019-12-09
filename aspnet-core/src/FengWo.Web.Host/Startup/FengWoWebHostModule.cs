using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using FengWo.Configuration;

namespace FengWo.Web.Host.Startup
{
    [DependsOn(
       typeof(FengWoWebCoreModule))]
    public class FengWoWebHostModule: AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public FengWoWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(FengWoWebHostModule).GetAssembly());
        }
    }
}
