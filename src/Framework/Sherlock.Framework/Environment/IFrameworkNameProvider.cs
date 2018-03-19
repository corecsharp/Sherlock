using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace Sherlock.Framework.Environment
{
    /// <summary>
    /// 平台名称提供程序。
    /// </summary>
    public interface IFrameworkNameProvider
    {
        /// <summary>
        /// 获取当前平台的名称。
        /// </summary>
        /// <returns></returns>
        FrameworkName GetCurrentName();
    }

    public class DefaultFrameworkNameProvider : IFrameworkNameProvider
    {
        public FrameworkName GetCurrentName()
        {
#if COREFX
            return new FrameworkName(".NetCore", new Version(1, 0));
#else
            return new FrameworkName(".NetFramework", System.Environment.Version);
#endif
        }
    }
}
