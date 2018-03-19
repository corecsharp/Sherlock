using Microsoft.Extensions.Logging;
using Sherlock.Framework.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sherlock.Framework;
using Sherlock.Framework.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sherlock.Framework.Environment;
using System.ComponentModel;
using Sherlock.Framework.Services;

namespace Sherlock.Framework
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class DapperLogExtensions
    {
        /// <summary>
        /// 添加 Dapper 数据库日志。
        /// </summary>
        /// <param name="loggerFactory"></param> 
        /// <param name="filter">日志记录条件过滤。</param>
        public static void AddDapper(this ILoggerFactory loggerFactory, Func<string, LogLevel, bool> filter)
        {
            var workContextAccessor = SherlockEngine.Current.GetRequiredService<IWorkContextAccessor>();
            var options = SherlockEngine.Current.GetRequiredService <IOptions<SherlockOptions>>();
            var idSvc = SherlockEngine.Current.GetRequiredService<IIdGenerationService>();

            var provider = new DapperLoggerProvider(idSvc, workContextAccessor, options, filter);
            loggerFactory.AddProvider(provider);
        }

        /// <summary>
        /// 添加 Dapper 数据库日志。
        /// </summary>
        /// <param name="loggerFactory"></param> 
        /// <param name="miniLevel">指定一个日志级别，只有日志级别大于该值的日志才进行记录。</param>
        public static void AddDapper(this ILoggerFactory loggerFactory, LogLevel miniLevel = LogLevel.Information)
        {
            loggerFactory.AddDapper((category, level) => level >= miniLevel);
        }
    }
}
