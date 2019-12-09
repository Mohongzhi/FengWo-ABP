using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using FengWo.Configuration;
using FengWo.BackgroundWorker;
using Abp.Threading.BackgroundWorkers;
using Abp.Hangfire;
using Abp.BackgroundJobs;

namespace FengWo.Web.Startup
{
    [DependsOn(typeof(FengWoWebCoreModule),
        typeof(AbpHangfireAspNetCoreModule),
        typeof(FengWoBackgoundWorkerModule)

        )]
    public class FengWoWebMvcModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public FengWoWebMvcModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.Navigation.Providers.Add<FengWoNavigationProvider>();

            //var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            //workManager.Add(IocManager.Resolve<SynchronizeUserInfoWorker>());
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(FengWoWebMvcModule).GetAssembly());
        }
    }
}
