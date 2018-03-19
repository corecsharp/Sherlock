using Microsoft.Extensions.Configuration;
using Sherlock.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework
{
    public static class JsonContentConfigurationExtensions
    {
        /// <summary>
        /// 添加一个 Json 文本配置。
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="jsonString">包含 Json 配置的 Json 文本。</param>
        /// <returns></returns>
        public static IConfigurationBuilder AddJsonContent(
                this IConfigurationBuilder configuration,
                string jsonString)
        {
            if (!jsonString.IsNullOrWhiteSpace())
            {
                configuration.Add(new JsonContentConfigurationSource(jsonString));
            }
            return configuration;
        }

    }
}
