using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace Sherlock.Framework.Environment
{
    /// <summary>
    /// 实现此接口以供应用程序识别当前运行环境。
    /// </summary>
    public interface ISherlockEnvironment 
    {
        /// <summary>
        /// 获取当前环境名称。
        /// 生产 production
        /// 开发 development
        /// 预发布 prerelease
        /// 测试 testing
        /// </summary>
        string Environment { get; }

        /// <summary>
        /// 获取一个值，只是是否应用正处于开发环境。
        /// </summary>
        bool IsDevelopmentEnvironment { get; }

        /// <summary>
        /// 获取应用程序基础路径。
        /// </summary>
        string ApplicationBasePath { get; }
        /// <summary>
        /// 获取应用程序版运行时框架。
        /// </summary>
        FrameworkName RuntimeFramework { get; }

        /// <summary>
        /// 获取应用程序实例的 Id。
        /// </summary>
        string ApplicationInstanceId { get; }
    }
}
