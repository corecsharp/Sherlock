using Microsoft.Extensions.Configuration;
using Sherlock.Framework.Caching;
using System;
using Microsoft.Extensions.DependencyInjection;
using Sherlock.Framework.DependencyInjection;
using Microsoft.Extensions.Options;
using Sherlock.Framework;

namespace Sherlock
{
    public static class RedisCacheExtensions
    {
        private static Guid _module = Guid.NewGuid();

        /// <summary>
        /// 使用 Redis 缓存服务（将以 Redis 缓存实现 <see cref="ICacheManager"/> 接口，默认连接配置节为 Sherlock:Redis。）。
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="setup"> Redis 缓存的配置安装方法。</param>
        /// <returns></returns>
        public static SherlockServicesBuilder AddRedisCache(this SherlockServicesBuilder builder, Action<SherlockRedisOptions> setup = null)
        {
            var configuration = builder.Configuration.GetSection("Sherlock:Redis") as IConfiguration ?? new ConfigurationBuilder().Build();
            
            builder.ServiceCollection.Configure<SherlockRedisOptions>(configuration);

            SherlockRedisOptions options = new SherlockRedisOptions();
            var redisSetup = new ConfigureFromConfigurationOptions<SherlockRedisOptions>(configuration);
            redisSetup.Configure(options);
            if (setup != null)
            {
                setup(options);
                builder.ServiceCollection.Configure(setup);
            }
            
            builder.ServiceCollection.AddSmart(ServiceDescriber.Singleton<ICacheManager, RedisCacheManager>(SmartOptions.Replace));
            if (builder.AddedModules.Add(_module))
            {
                builder.ServiceCollection.AddSmart(ServiceDescriber.Singleton<IRedisCacheSerializer, JsonNetSerializer>(SmartOptions.Append));
            }

            if (options.ConnectionString.IsNullOrWhiteSpace())
            {
                throw new SherlockException("必须为 RedisCacheManager 指定连接字符串，可以通过 Sherlock:Redis:ConnectionString 配置节配置。");
            }
            
            return builder;
        }

        /// <summary>
        /// 配置 Sherlock 框架的 Redis 缓存服务。
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="setup">表示配置操作的方法。</param>
        public static void ConfigureRedisCache(this IServiceCollection builder, Action<SherlockRedisOptions> setup = null)
        {
            if (setup != null)
            {
                builder.Configure(setup);
            }
        }
    }
}
