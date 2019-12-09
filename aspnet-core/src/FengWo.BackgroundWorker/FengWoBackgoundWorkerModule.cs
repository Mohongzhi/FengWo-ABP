using Abp.Hangfire.Configuration;
using Abp.Reflection.Extensions;
using Abp.Modules;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Text;
using Abp.BackgroundJobs;
using Abp.Hangfire;
using Abp.Threading.BackgroundWorkers;
using FengWo.BackgroundWorker.Hangfire;
using Abp.Dependency;
using Castle.MicroKernel.Registration;

namespace FengWo.BackgroundWorker
{
    [DependsOn(typeof(AbpHangfireAspNetCoreModule))]
    public class FengWoBackgoundWorkerModule : AbpModule
    {

        public override void PreInitialize()
        {
            Configuration.BackgroundJobs.UseHangfire();
            //IocManager.RegisterIfNot<IBackgroudWorkerProxy, PeriodicWorkerPxoxy>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(FengWoBackgoundWorkerModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            //////判断是否启用了hangfire，如果启用了，则将IBackgroudWorkerProxy的实例改为hangfire
            //var hangfireConfig = IocManager.Resolve<IAbpHangfireConfiguration>();
            //if (hangfireConfig?.Server != null)
            //{
            //    IocManager.IocContainer.Register(Component.For<IBackgroudWorkerProxy>().ImplementedBy<HangfireWorkerPxoxy>().IsDefault());
            //}
            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            workManager.Add(IocManager.Resolve<SynchronizeUserInfoWorker>());

        }
    }
}
