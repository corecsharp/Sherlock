using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Sherlock.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.Web.DependencyInjection
{
    public class SherlockWebBuilder
    {
        private SherlockServicesBuilder _builder = null;

        public SherlockWebBuilder(SherlockServicesBuilder builder)
        {
            _builder = builder;
        }

        internal Action<IMvcBuilder> MvcSetup { get; set; }
        internal Action<IMvcCoreBuilder> MvcCoreSetup { get; set; }
        internal Action<SherlockWebOptions> FeatureSetup { get; set; }

        internal Action<CookieAuthenticationOptions> CookieSetup { get; set; }

        internal List<WebStarter> WebStarters { get; } = new List<WebStarter>();

        public HashSet<Guid> AddedModules { get; } = new HashSet<Guid>();


        public SherlockWebBuilder ConfigureServices(Action<IServiceCollection> configre)
        {
            configre?.Invoke(_builder.ServiceCollection);
            return this;
        }

        /// <summary>
        /// 添加 Web 启动器。
        /// </summary>
        /// <param name="starter">要添加的启动器。</param>
        /// <returns></returns>
        public SherlockWebBuilder AddStarter(WebStarter starter)
        {
            this.WebStarters.Add(starter);
            return this;
        }

        /// <summary>
        /// 配置 Sherlock Web 功能选项。
        /// </summary>
        /// <param name="setup"></param>
        /// <returns></returns>
        public SherlockWebBuilder ConfigureFeature(Action<SherlockWebOptions> setup)
        {
            this.FeatureSetup += setup;
            return this;
        }

        /// <summary>
        /// 对 ASPNET MVC 进行配置。
        /// </summary>
        /// <param name="mvcSetup"></param>
        /// <returns></returns>
        public SherlockWebBuilder ConfigureMvc(Action<IMvcBuilder> mvcSetup)
        {
            this.MvcSetup += mvcSetup;
            return this;
        }

        /// <summary>
        /// 对 ASPNET MVC Core 进行配置（仅当 <see cref="MvcFeatures.Api"/> 或 <see cref="MvcFeatures.Core"/>  有效）。
        /// </summary>
        /// <param name="mvcSetup"></param>
        /// <returns></returns>
        public SherlockWebBuilder ConfigureMvcCore(Action<IMvcCoreBuilder> mvcSetup)
        {
            this.MvcCoreSetup += mvcSetup;
            return this;
        }

        /// <summary>
        /// 配置 Cookie 和 基于 Cookie 的身份认证的相关的选项。
        /// </summary>
        /// <param name="cookieSetup"></param>
        /// <returns></returns>
        public SherlockWebBuilder ConfigureCookie(Action<CookieAuthenticationOptions> cookieSetup)
        {
            this.CookieSetup += cookieSetup;
            return this;
        }
    }
}
