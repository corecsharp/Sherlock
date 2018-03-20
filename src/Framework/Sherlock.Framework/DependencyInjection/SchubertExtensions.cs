using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Sherlock.Framework;
using Sherlock.Framework.Caching;
using Sherlock.Framework.Components;
using Sherlock.Framework.DependencyInjection;
using Sherlock.Framework.Environment;
using Sherlock.Framework.Environment.ShellBuilders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sherlock
{
    public static class SherlockExtensions
    {
        /// <summary>
        /// 向服务集合中添加 SherlockFramework 的依赖服务。
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">应用程序配置对象。</param>
        /// <param name="setupAction">SherlockFramework 安装选项。</param>
        /// <param name="shellCreationSetup">配置 Shell 环境，该操作作用域仅为 Shell 创建过程。</param>
        public static void AddSherlockFramework(this IServiceCollection services, IConfiguration configuration,
            Action<SherlockServicesBuilder> setupAction = null,
            Action<ShellCreationScope> shellCreationSetup = null)
        {
            AddEnvironmentVariables(configuration);

            IConfiguration c = (configuration.GetSection("Sherlock") as IConfiguration) ?? new ConfigurationBuilder().Build();
#if COREFX
            //.net core 控制台BUG
            Console.OutputEncoding = System.Text.Encoding.UTF8;
#endif
            Guard.ArgumentNotNull(configuration, nameof(configuration));

            SherlockEngine.Current.ApplicationName = c.GetSection(nameof(SherlockOptions.AppSystemName))?.Value.IfNullOrWhiteSpace(String.Empty);
            SherlockEngine.Current.GroupName = c.GetSection(nameof(SherlockOptions.Group))?.Value.IfNullOrWhiteSpace(String.Empty);
            SherlockEngine.Current.ApplicationVersion = c.GetSection(nameof(SherlockOptions.Version))?.Value.IfNullOrWhiteSpace("0.0.0");
            services.Configure<MemoryCacheOptions>(o => o.ExpirationScanFrequency = TimeSpan.FromMinutes(3));
            //刷新和ExpirationScanFrequency的设置没有关系
            services.Configure<SherlockOptions>(c);

            var networkConfiguration = (configuration.GetSection("Sherlock:Network") as IConfiguration) ?? new ConfigurationBuilder().Build();
            services.Configure<NetworkOptions>(networkConfiguration);

            var builder = new SherlockServicesBuilder(services, configuration);
            setupAction?.Invoke(builder);

            InitShell(builder, shellCreationSetup);
        }

        private static void AddEnvironmentVariables(IConfiguration configuration)
        {
            var envBuilder = new ConfigurationBuilder();
            try
            {
                envBuilder.AddEnvironmentVariables();
                envBuilder.AddCommandLine();
            }
            catch (NotSupportedException)
            { }

            var envConfig = envBuilder.Build();

            var keyValues = envConfig.AsEnumerable();

            foreach (var kv in keyValues)
            {
                configuration[kv.Key] = kv.Value;
            }
        }

        public static void AddCommandLine(this IConfigurationBuilder envBuilder)
        {
            var args = Environment.GetCommandLineArgs();
            if (args != null && args.Length > 1)
            {
                String[] dest = new String[args.Length - 1];
                Array.Copy(args, 1, dest, 0, dest.Length);
                envBuilder.AddCommandLine(dest);
            }
        }

        private static IServiceCollection FillToOther(this IServiceCollection collection, IServiceCollection dest = null)
        {
            int index = 0;
            IServiceCollection result = dest ?? new ServiceCollection();
            foreach (var s in collection)
            {
                result.Insert(index, s);
                index++;
            }
            return result;
        }

        private static void InitShell(SherlockServicesBuilder SherlockBuilder, Action<ShellCreationScope> setup)
        {

            var builder = SherlockBuilder.ServiceCollection.FillToOther();
            builder.AddSmart(SherlockServices.GetServices(SherlockBuilder.Configuration));

            var scopeFactory = builder.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();
            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                IServiceProvider provider = scope.ServiceProvider;

                ShellCreationScope shellScope = new ShellCreationScope(provider);
                setup?.Invoke(shellScope);
                LogLevel level = SherlockUtility.InVisualStudio() ? LogLevel.Trace : LogLevel.Information;
                shellScope.LoggerFactory.AddConsole((n, l) => l >= level && n.StartsWith("Sherlock"));
                var shellLogger = shellScope.LoggerFactory.CreateLogger("Sherlock");

                shellLogger.WriteInformation("开始加载 Shell。");

                var sw = Stopwatch.StartNew();
                SherlockEngine.Current.LoadEnvironment(provider);
                var factory = provider.GetRequiredService<IShellContextFactory>();
                var context = factory.CreateShellContext();

                AddConfiguredOptions(SherlockBuilder.ServiceCollection, SherlockBuilder.Configuration, context);

                SherlockBuilder.ServiceCollection.AddSingleton(context);
                SherlockBuilder.ServiceCollection.AddSmart(context.Services);
                SherlockBuilder.ServiceCollection.AddSmart(SherlockServices.GetServices(SherlockBuilder.Configuration));

                SherlockEngine.Current.ShellCreated = true;

                IOptions<SherlockOptions> SherlockOptions = provider.GetRequiredService<IOptions<SherlockOptions>>();

                context.RegisteredServices = builder;
                ShellEvents.NotifyShellInitialized(SherlockOptions.Value, context);
                sw.Stop();


                var table = new Tuple<int, int, int, int, String>(
                    context.Blueprint.Modules.Count(),
                    context.Blueprint.Controllers.Count(),
                    context.Blueprint.Dependencies.Count(),
                    context.Blueprint.DependencyDescribers.Count(),
                    context.Blueprint.Modules.ToArrayString(System.Environment.NewLine));

                StringBuilder info = new StringBuilder();
                info.AppendLine(@"
 ________  ________  ___  ___  ___  ___  ________  _______   ________  _________   
|\   ____\|\   ____\|\  \|\  \|\  \|\  \|\   __  \|\  ___ \ |\   __  \|\___   ___\ 
\ \  \___|\ \  \___|\ \  \\\  \ \  \\\  \ \  \|\ /\ \   __/|\ \  \|\  \|___ \  \_| 
 \ \_____  \ \  \    \ \\   __  \ \  \\\  \ \   __  \ \  \_|/_\ \   _  _\   \ \  \  
  \|____|\  \ \  \____\ \  \ \  \ \  \\\  \ \  \|\  \ \  \_|\ \ \  \\  \|   \ \  \ 
    ____\_\  \ \_______\ \__\ \__\ \_______\ \_______\ \_______\ \__\\ _\    \ \__\
   |\_________\|_______|\|__|\|__|\|_______|\|_______|\|_______|\|__|\|__|    \|__|
   \|_________|                                                                         
                                                                                   ");

                info.AppendLine($"Shell 加载完成，Sherlock Version: {typeof(SherlockException).GetTypeInfo().Assembly.GetName().Version.ToString()} （{sw.ElapsedMilliseconds} ms）。");
                info.AppendLine("  ");
                info.AppendLine((new Tuple<int, int, int, int, String>[] { table }).ToStringTable(new String[] { "modules", "controllers", "dependencies", "describers", "moduleList" },
                    t => t.Item1, t => t.Item2, t => t.Item3, t => t.Item4, t => t.Item5));
                info.AppendLine("   ");
                info.Append(context.Blueprint.Dependencies.GroupBy(d => d.Feature.Descriptor.ModuleName, d => (ShellBlueprintDependencyItem)d).ToStringTable(
                    new string[] { "module", "dependencies", "internfaces", "lifetime" },
                    f => f.Key,
                    d => d.SelectMany(t => CreateArray(t.Type.Name, t.Interfaces.Count)).ToArrayString(System.Environment.NewLine),
                    d => d.SelectMany(t => t.Interfaces).Select(i => i.Item1.Name).ToArrayString(System.Environment.NewLine),
                    d => d.SelectMany(t => t.Interfaces).Select(i => i.Item2.ToString().ToLower()).ToArrayString(System.Environment.NewLine)));
                info.AppendLine("   ");

                shellLogger.WriteInformation(info.ToString());
            }
        }

        private static IEnumerable<T> CreateArray<T>(T item, int arrayCount)
        {
            List<T> list = new List<T>(arrayCount);
            for (int i = 0; i < arrayCount; i++)
            {
                list.Add(item);
            }
            return list;
        }

        private static void AddConfiguredOptions(IServiceCollection builder, IConfiguration configuration, ShellContext context)
        {
            Lazy<Type> configurationOptionsType = new Lazy<Type>(() => typeof(ConfigureFromConfigurationOptions<>));
            Lazy<Type> configurationInterfaceType = new Lazy<Type>(() => typeof(IConfigureOptions<>));
            foreach (var options in context.Blueprint.ConfiguredOptions)
            {
                Type interfaceType = configurationInterfaceType.Value.MakeGenericType(options.Type);
                Type opType = configurationOptionsType.Value.MakeGenericType(options.Type);
                IConfiguration config = configuration.GetSection(options.ConfigurationSection);
                object instance = System.Activator.CreateInstance(opType, args: config);
                builder.AddSingleton(interfaceType, instance);
            }
        }

        /// <summary>
        /// 配置使用乐观并发方式的唯一 Id 生成器。
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="setupAction"></param>
        [Obsolete("use 'Sherlock.Framework.Services.IIdGenerationService' instead")]
        public static void AddOptimisticUniqueIdOptions(this SherlockServicesBuilder collection, Action<OptimisticUniqueIdOptions> setupAction)
        {
            collection.ServiceCollection.AddSmart(ServiceDescriber.Singleton<IUniqueIdGenerator, OptimisticUniqueIdGenerator>());
            collection.ServiceCollection.Configure(setupAction);
        }

        /// <summary>
        /// 使用 Sherlock 提供的缓存来实现 asp.net 框架中的缓存（<see cref="IDistributedCache"/>）提供程序。
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="regionName">用来存储 asp.net 缓存数据的分区。</param>
        public static void AddCacheForAspNet(this SherlockServicesBuilder collection, string regionName = "aspnet")
        {
            collection.ServiceCollection.AddSmart(new SmartServiceDescriptor(typeof(IDistributedCache), serviceProvider =>
            {
                var manager = serviceProvider.GetRequiredService<ICacheManager>();
                return new DistributedCacheAdapter(manager, regionName);
            }, ServiceLifetime.Transient));
        }

        /// <summary>
        /// 使用 Sherlock 提供的缓存来实现 asp.net 框架中的缓存（<see cref="IDistributedCache"/>）提供程序。
        /// </summary>
        /// <param name="regionName">用来存储 asp.net 缓存数据的分区。</param>
        /// <param name="collection"></param>
        [Obsolete("use 'SherlockServicesBuilder.AddCacheForAspNet' instead")]
        public static void AddSherlockCache(this IServiceCollection collection, string regionName = "aspnet")
        {
            collection.AddSmart(new SmartServiceDescriptor(typeof(IDistributedCache), serviceProvider =>
            {
                var manager = serviceProvider.GetRequiredService<ICacheManager>();
                return new DistributedCacheAdapter(manager, regionName);
            }, ServiceLifetime.Transient));
        }

        public static void StartSherlockEngine(this IServiceProvider provider)
        {
            //启动引擎，为我们动态注册服务，创建 Shell 上下文。
            SherlockEngine.Current.Start(provider);
            IOptions<SherlockOptions> SherlockOptions = provider.GetRequiredService<IOptions<SherlockOptions>>();

            ShellEvents.NotifyEngineStarted(SherlockOptions.Value, provider);
            LogEngineStarted(provider);

        }

        private static void LogEngineStarted(IServiceProvider provider)
        {
            ILoggerFactory loggerFactory = provider.GetService<ILoggerFactory>();
            ILogger logger = loggerFactory?.CreateLogger("Sherlock") ?? NullLogger.Instance;

            ISherlockEnvironment SherlockEnvironment = provider.GetRequiredService<ISherlockEnvironment>();

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Sherlock engine was started.");
            var properties = SherlockEnvironment.ToDictionary();
            foreach (var kv in properties)
            {
                builder.AppendLine($"{kv.Key}: {kv.Value}");
            }
            logger.WriteInformation(builder.ToString());
        }
    }
}