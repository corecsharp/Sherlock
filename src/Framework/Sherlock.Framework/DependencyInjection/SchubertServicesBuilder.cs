using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sherlock.Framework.Environment.Modules;
using System;
using System.Collections.Generic;

namespace Sherlock.Framework.DependencyInjection
{
    /// <summary>
    /// 构建 Sherlock 运行环境的 Fluent 编程对象。
    /// </summary>
    public sealed class SherlockServicesBuilder
    {
        private IServiceCollection _serviceCollection;
        private IConfiguration _configuration;

        public HashSet<Guid> AddedModules { get; } = new HashSet<Guid>();

        /// <summary>
        /// 创建 <see cref="SherlockServicesBuilder"/> 类的新实例。
        /// </summary>
        internal SherlockServicesBuilder(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            Guard.ArgumentNotNull(serviceCollection, nameof(serviceCollection));
            Guard.ArgumentNotNull(configuration, nameof(configuration));

            _serviceCollection = serviceCollection;
            _configuration = configuration;
        }

        /// <summary>
        /// 配置框架参数。
        /// </summary>
        /// <param name="setup"><see cref="Action{T}"/> 对象。</param>
        public void ConfigureOptions(Action<SherlockOptions> setup)
        {
            if (setup != null)
            {
                _serviceCollection.Configure<SherlockOptions>(setup);
            }
        }

        /// <summary>
        /// 配置网络参数。
        /// </summary>
        /// <param name="setup"><see cref="Action{T}"/> 对象。</param>
        public void ConfigureNetwork(Action<NetworkOptions> setup)
        {
            if (setup != null)
            {
                _serviceCollection.Configure(setup);
            }
        }

        public SherlockServicesBuilder UserShellDescriptorManager<TManager>()
            where TManager : IShellDescriptorManager
        {
            this.ServiceCollection.TryAdd(new ServiceDescriptor(typeof(IShellDescriptorManager), typeof(TManager), ServiceLifetime.Transient));
            return this;
        }

       

        public IConfiguration Configuration
        {
            get { return _configuration; }
        }

        public IServiceCollection ServiceCollection
        {
            get { return _serviceCollection; }
        }
    }
}