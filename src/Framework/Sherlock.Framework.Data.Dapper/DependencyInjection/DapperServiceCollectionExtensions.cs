using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sherlock.Framework.DependencyInjection;
using Sherlock.Framework.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sherlock.Framework.Logging;
using Microsoft.Extensions.Logging;
using Sherlock.Framework.Data.DependencyInjection;

namespace Sherlock
{

    public static class DapperServiceCollectionExtensions
    {
        /// <summary>
        /// 添加以 Dapper 作为持久化的数据层特性。
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="setup"></param>
        /// <returns></returns>
        public static SherlockServicesBuilder AddDapperDataFeature(this SherlockServicesBuilder builder, Action<DapperDataFeatureBuilder> setup = null)
        {
            //修改dapper的默认映射规则,让其支持下划线列名到C#实体驼峰命名属性
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
             


            var configuration = builder.Configuration.GetSection("Sherlock:Data") as IConfiguration ?? new ConfigurationBuilder().Build();

            builder.ServiceCollection.Configure<DapperDatabaseOptions>(configuration);

            DapperDatabaseOptions dbOptions = new DapperDatabaseOptions();
            var SherlockDataSetup = new ConfigureFromConfigurationOptions<DapperDatabaseOptions>(configuration);
            SherlockDataSetup.Configure(dbOptions);

            DapperDataFeatureBuilder featureBuilder = new DapperDataFeatureBuilder(dbOptions);

            setup?.Invoke(featureBuilder);

            
            featureBuilder.Build();
            builder.ServiceCollection.Configure(featureBuilder.Configure);

            builder.ServiceCollection.AddSmart(DapperServices.GetServices(dbOptions));
            return builder;
        }

        /// <summary>
        /// 添加默认的 Dapper Logger 的数据库配置（使用 Dapper 数据库记录日志必须调用此方法或自己实现 <see cref="LogData"/> 元数据映射）。
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="connectionName">连接名</param>
        /// <param name="logTableName">日志表名</param>
        /// <returns></returns>
        public static SherlockServicesBuilder AddDapperLogger(this SherlockServicesBuilder builder, string logTableName = "Logs", string connectionName = null)
        {
            var metadataDescriptor = ServiceDescriber.Describe(typeof(IDapperMetadataProvider), s=>new LogDataMetadataProvier(logTableName), ServiceLifetime.Singleton);
           
            var repositoryDescriptor = ServiceDescriber.Describe(typeof(IRepository<LogData>), 
                s => {
                    var context = s.GetRequiredService<DapperContext>();
                    var loggerFactory = s.GetRequiredService<ILoggerFactory>();
                    return new LogDataRepository(connectionName, context, loggerFactory);
                },
                ServiceLifetime.Scoped);

            builder.ServiceCollection.AddSmart(metadataDescriptor);
            builder.ServiceCollection.AddSmart(repositoryDescriptor);

            return builder;
        }

    }
}
