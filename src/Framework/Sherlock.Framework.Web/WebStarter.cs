using Microsoft.AspNetCore.Builder;
using Sherlock.Framework.DependencyInjection;
using Sherlock.Framework.Web.DependencyInjection;

namespace Sherlock.Framework.Web
{
    /// <summary>
    /// 表示一个  Web 启动器项。
    /// </summary>
    public abstract class WebStarter
    {
        public abstract void ConfigureServices(SherlockServicesBuilder servicesBuilder, SherlockWebOptions options);

        public abstract void Start(IApplicationBuilder appBuilder, SherlockWebOptions options);
    }
}
