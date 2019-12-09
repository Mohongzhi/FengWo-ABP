using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Castle.Facilities.Logging;
using Abp.AspNetCore;
using Abp.Castle.Logging.Log4Net;
using FengWo.Authentication.JwtBearer;
using FengWo.Configuration;
using FengWo.Identity;
using FengWo.Web.Resources;
using Abp.AspNetCore.SignalR.Hubs;
using FengWo.Web.Hubs;
using Hangfire;
using Hangfire.MySql.Core;
using Abp.Hangfire;
using FengWo.Authorization;

namespace FengWo.Web.Startup
{
    public class Startup
    {
        private readonly IConfigurationRoot _appConfiguration;

        public Startup(IHostingEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // MVC
            services.AddMvc(
                options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute())
            );

            IdentityRegistrar.Register(services);
            AuthConfigurer.Configure(services, _appConfiguration);

            services.AddScoped<IWebResourceManager, WebResourceManager>();

            services.AddSignalR();

            services.AddHangfire(config =>
            {
                config.UseStorage(new MySqlStorage(_appConfiguration.GetConnectionString("Default")));
            });

            // Configure Abp and Dependency Injection
            return services.AddAbp<FengWoWebMvcModule>(
                // Configure Log4Net logging
                options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                )
            );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAbp(); // Initializes ABP framework.

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseJwtTokenMiddleware();

            app.UseSignalR(routes =>
            {
                routes.MapHub<AbpCommonHub>("/signalr");
                routes.MapHub<ChatHub>("/signalr-chatHub");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaultWithArea",
                    template: "{area}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new AbpHangfireAuthorizationFilter(PermissionNames.Pages_Administration_HangfireDashboard) }
            });

            app.UseHangfireServer();//开启Hangfire
        }
    }
}
