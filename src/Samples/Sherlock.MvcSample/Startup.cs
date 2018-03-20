using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Sherlock.MvcSample
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(env.ContentRootPath)
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
#if !DEBUG
             .AddConfigurationCenter(env.ContentRootPath)
#endif
          //TODO 注册中心连不上的时候要警告 
          .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddSherlockFramework(Configuration,
                builder =>
                {

                    builder.AddWebFeature(web =>
                    {
                        web.ConfigureFeature(settings =>
                        {
                            settings.MvcFeatures = Sherlock.Framework.Web.MvcFeatures.Api;
                            settings.JsonSerializeLongAsString = true;
                        });

                    });
                    builder.AddDapperDataFeature();
                },
                scope =>
                {
                    scope.LoggerFactory.AddConsole(LogLevel.Error);
                    //scope.LoggerFactory.AddFile("d:\\start_log.txt");
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(LogLevel.Warning);
            app.StartSherlockWebApplication();
        }
    }
}
