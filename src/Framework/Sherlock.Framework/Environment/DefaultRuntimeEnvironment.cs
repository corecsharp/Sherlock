
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Versioning;

namespace Sherlock.Framework.Environment
{
    /// <summary>
    /// 以 DEBUG 编译符号来作为判断开发环境标准的提供程序。
    /// </summary>
    public class DefaultRuntimeEnvironment : ISherlockEnvironment
    {
        private IInstanceIdProvider _instanceIdProvider = null;
        private IFrameworkNameProvider _frameworkNameProvider = null;

        public DefaultRuntimeEnvironment(String complieConfiguration, IFrameworkNameProvider frameworkNameProvider, IInstanceIdProvider instanceIdProvider)
        {
            Guard.ArgumentNotNull(instanceIdProvider, nameof(instanceIdProvider));
            _frameworkNameProvider = frameworkNameProvider ?? new DefaultFrameworkNameProvider();

            _instanceIdProvider = instanceIdProvider;
            this.IsDevelopmentEnvironment = (complieConfiguration.IfNullOrWhiteSpace(String.Empty)).CaseInsensitiveEquals("development");
            this.Environment = complieConfiguration.IfNullOrWhiteSpace("Production").ToLower();
            this.ApplicationBasePath = SherlockUtility.GetApplicationDirectory();
            this.RuntimeFramework = _frameworkNameProvider.GetCurrentName() ?? new FrameworkName("UNKNOWN", new Version(0, 0, 0));

        }

        public string ApplicationInstanceId
        {
            get { return _instanceIdProvider.GetInstanceId(); }
        }

        public string Environment { get; }

        public bool IsDevelopmentEnvironment { get; }

        public string ApplicationBasePath { get; }
        public string Configuration { get; }
        public FrameworkName RuntimeFramework { get; }
    }
}
