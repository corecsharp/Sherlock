using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Sherlock.Framework.Caching;
using Sherlock.Framework.DependencyInjection;
using Sherlock.Framework.Environment.Modules;
using Sherlock.Framework.Json;
using Sherlock.Framework.Localization;
using Sherlock.Framework.Web;
using Sherlock.Framework.Web.Authentication;
using Sherlock.Framework.Web.DependencyInjection;
using Sherlock.Framework.Web.FileProviders;
using Sherlock.Framework.Web.Mvc;
using System;
using System.Linq;

namespace Sherlock
{
    public static class SherlockWebExtensions
    {
        public static SherlockWebBuilder _webBuilder = null;
        private static Guid _module = Guid.NewGuid();

        /// <summary>
        /// 启用Sherlock 框架的 Web 特性。
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setup">加入 Mvc 支持。</param>
        /// <returns></returns>
        public static SherlockServicesBuilder AddWebFeature(this SherlockServicesBuilder services, Action<SherlockWebBuilder> setup = null)
        {
            SherlockWebOptions options = new SherlockWebOptions();
            bool firstInvoke = true;
            if ((firstInvoke = services.AddedModules.Add(_module)))
            {
                IConfiguration configuration = services.Configuration.GetSection("Sherlock:Web") as IConfiguration ?? new ConfigurationBuilder().Build();

                services.ServiceCollection.Configure<SherlockWebOptions>(configuration);

                var SherlockWebSetup = new SherlockWebOptionsSetup(configuration);
                SherlockWebSetup.Configure(options);
            }

            _webBuilder = new SherlockWebBuilder(services);
            setup?.Invoke(_webBuilder);


            if (_webBuilder.FeatureSetup != null)
            {
                services.ServiceCollection.Configure(setup);
            }
            _webBuilder.FeatureSetup?.Invoke(options);
            services.ServiceCollection.AddDataProtection();
            
            services.ServiceCollection.AddLocalization();
            services.ServiceCollection.Replace(ServiceDescriptor.Singleton<IStringLocalizerFactory, SherlockStringLocalizerFactory>());
            services.ServiceCollection.TryAddSingleton<IMemoryCache>(s=> LocalCache.InnerCache);
            services.AddCacheForAspNet();

            var cookieSetup = _webBuilder.CookieSetup;
            services.ServiceCollection.ConfigureApplicationCookie(o =>
            {
                o.LoginPath = "/Login";
                o.LogoutPath = "/LogOff";
                o.Cookie.HttpOnly = true;
                o.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                cookieSetup?.Invoke(o);
            });

            var authenticationBuilder = services.ServiceCollection.AddAuthentication();

            if (options.UseCookieAuth)
            {
                authenticationBuilder.AddCookie();
            }
            if (options.UseSession)
            {
                services.ServiceCollection.AddSession(sop =>
                {
                    sop.IdleTimeout = TimeSpan.FromMinutes(options.SessionTimeoutMinutes);
                });
            }
            
            services.ServiceCollection.AddSmart(SherlockWebServices.GetServices(options, firstInvoke));

            foreach (var s in _webBuilder.WebStarters)
            {
                s.ConfigureServices(services, options);
            }

            AddMvc(services, _webBuilder, options);

            return services;
        }
        

        private static IContractResolver GetContractResolver(JsonCaseStyle style, JsonResolverSettings settings)
        {
            switch (style)
            {
                case JsonCaseStyle.CamelCase:
                   
                    return new ExtendedCamelCaseContractResolver(settings);
                case JsonCaseStyle.PascalCase:
                    default:
                    return new ExtendedContractResolver(settings);
            }
        }
        

        private static void AddMvc(SherlockServicesBuilder services, SherlockWebBuilder featureBuilder, SherlockWebOptions options)
        {
            switch (options.MvcFeatures)
            {
                case MvcFeatures.Full:
                    var mvcBuilder = services.ServiceCollection.AddMvc();
                    featureBuilder.MvcSetup?.Invoke(mvcBuilder);
                    mvcBuilder.AddJsonOptions(json=>json.SerializerSettings.ContractResolver = GetContractResolver(options.JsonCaseStyle, options.JsonResolver))
                        .AddRazorOptions(rveo =>
                        {
                            rveo.FileProviders.Insert(0, new ModuleFileProvider(rveo.FileProviders.FirstOrDefault()));
                            rveo.ViewLocationExpanders.Insert(0, new ModuleViewLocationExpander());
                        });
                    //services.ServiceCollection.AddAntiforgery();
                    break;
                case MvcFeatures.Core:
                    var coreBuilder = services.ServiceCollection.AddMvcCore();
                    featureBuilder.MvcCoreSetup?.Invoke(coreBuilder);
                    break;
                case MvcFeatures.Api:
                    var apiBuilder = services.ServiceCollection.AddMvcCore();
                    featureBuilder.MvcCoreSetup?.Invoke(apiBuilder);
                    apiBuilder.AddApiExplorer()
                        .AddAuthorization()
                        .AddFormatterMappings()
                        .AddJsonFormatters(settings=>settings.ContractResolver = GetContractResolver(options.JsonCaseStyle, options.JsonResolver))
                        .AddDataAnnotations()
                        .AddCors()
                        .AddWebApiConventions();

                    break;
                default:
                    return;
            }
        }

        /// <summary>
        /// 启动基于 Sherlock 引擎的 Web 应用程序。
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder StartSherlockWebApplication(this IApplicationBuilder builder)
        {
            builder.ApplicationServices.StartSherlockEngine();
            //启动引擎，为我们动态注册服务，创建 Shell 上下文。
            IOptions<SherlockWebOptions> options = builder.ApplicationServices.GetService<IOptions<SherlockWebOptions>>();
            if (options != null && options.Value != null)
            {
                foreach (var s in _webBuilder.WebStarters)
                {
                    s.Start(builder, options.Value);
                }

                var moduleManager = builder.ApplicationServices.GetRequiredService<IModuleManager>();
                
                builder.UseStaticFiles();
                builder.UseAuthentication();

                if (options.Value.UseSession)
                {
                    builder.UseSession();
                }
                if (options.Value.MvcFeatures != MvcFeatures.None)
                {
                    builder.UseMvc();
                }
            }
            //尽可能节省内存，让GC可以回收 WebBuilder
            _webBuilder = null;
            return builder;
        }
    }
}