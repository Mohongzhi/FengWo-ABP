using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using FengWo.Authorization;

namespace FengWo
{
    [DependsOn(
        typeof(FengWoCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class FengWoApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<FengWoAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(FengWoApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );
        }
    }
}
