using Abp.Modules;
using Abp.Reflection.Extensions;
using FengWo.Configuration;
using FengWo.Web;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace FengWo.DBFactory
{
    public class FengWoDBFactoryModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IDBHelper, MySqlDBHelper>(Abp.Dependency.DependencyLifeStyle.Transient);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(FengWoDBFactoryModule).GetAssembly());
        }

        public override void PostInitialize()
        {
          
        }
    }
}
