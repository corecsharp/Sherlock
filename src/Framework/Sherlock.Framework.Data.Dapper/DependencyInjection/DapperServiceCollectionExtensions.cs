﻿using System;
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
        private static Guid _module = Guid.NewGuid();

        /// <summary>
        /// 添加以 Dapper 作为持久化的数据层特性。
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="setup"></param>
        /// <returns></returns>
        public static SherlockServicesBuilder AddDapperDataFeature(this SherlockServicesBuilder builder, Action<DapperDataFeatureBuilder> setup = null)
        {
            DapperDatabaseOptions dbOptions = new DapperDatabaseOptions();
            if (builder.AddedModules.Add(_module))
            {
                //修改dapper的默认映射规则,让其支持下划线列名到C#实体驼峰命名属性
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                var configuration = builder.Configuration.GetSection("Sherlock:Data") as IConfiguration ?? new ConfigurationBuilder().Build();

                builder.ServiceCollection.Configure<DapperDatabaseOptions>(configuration);

                var SherlockDataSetup = new ConfigureFromConfigurationOptions<DapperDatabaseOptions>(configuration);
                SherlockDataSetup.Configure(dbOptions);
            }

            DapperDataFeatureBuilder featureBuilder = new DapperDataFeatureBuilder(dbOptions);

            setup?.Invoke(featureBuilder);

            
            featureBuilder.Build();
            builder.ServiceCollection.Configure(featureBuilder.Configure);

            builder.ServiceCollection.AddSmart(DapperServices.GetServices(dbOptions));
            return builder;
        }

    }
}
