using Microsoft.Extensions.Logging;
using Sherlock.Framework.FileSystem.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Sherlock.Framework.Environment;

namespace Sherlock.Framework.DependencyInjection
{
    /// <summary>
    /// 表示创建 Shell 的环境配置。
    /// </summary>
    public sealed  class ShellCreationScope
    {
        private ILoggerFactory _loggerFactory = null;
        private IAppDataFolder _appDataForlder = null;
        private ISherlockEnvironment _enviroment = null;

        internal ShellCreationScope(IServiceProvider serviceProvider)
        {
            Guard.ArgumentNotNull(serviceProvider, nameof(serviceProvider));
            this.ServiceProvider = serviceProvider;
        }
        public IServiceProvider ServiceProvider { get; private set; }

        public ISherlockEnvironment Environment
        {
            get { return _enviroment ?? (_enviroment = ServiceProvider.GetRequiredService<ISherlockEnvironment>());  }
        }

        /// <summary>
        /// 获取 Shell 创建环境中的日志工厂。
        /// </summary>
        public ILoggerFactory LoggerFactory
        {
            get { return _loggerFactory ?? (_loggerFactory = ServiceProvider.GetRequiredService<ILoggerFactory>()); }
        }

        /// <summary>
        /// 获取 Shell 创建环境中的 AppData 目录。
        /// </summary>
        public IAppDataFolder AppDataFolder
        {
            get { return _appDataForlder ?? (_appDataForlder = ServiceProvider.GetRequiredService<IAppDataFolder>()); }
        }
    }
}
