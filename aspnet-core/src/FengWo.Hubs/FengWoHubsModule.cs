using Abp.AspNetCore.SignalR;
using Abp.Modules;
using Abp.Reflection.Extensions;
using FengWo.Web.Hubs;

namespace FengWo.Hubs
{
    [DependsOn(typeof(AbpAspNetCoreSignalRModule))]
    public class FengWoHubsModule : AbpModule
    {
        public override void PreInitialize()
        {
           // IocManager.Register<ChatHub, ChatHub>(Abp.Dependency.DependencyLifeStyle.Transient);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(FengWoHubsModule).GetAssembly());
        }

        public override void PostInitialize()
        {

        }
    }
}
