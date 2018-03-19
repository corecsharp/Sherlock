using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sherlock.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
