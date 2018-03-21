using Microsoft.AspNetCore.Hosting;
using Sherlock.Framework.Environment;
using System;
using System.Reflection;
using System.Runtime.Versioning;

namespace Sherlock.Framework.Web
{
    public class AspNetEnvironment : ISherlockEnvironment
    {
        private IInstanceIdProvider _instanceIdProvider = null;
        public AspNetEnvironment(IHostingEnvironment hosting,  IInstanceIdProvider instanceIdProvider)
        {
            Guard.ArgumentNotNull(hosting, nameof(hosting));
            Guard.ArgumentNotNull(instanceIdProvider, nameof(instanceIdProvider));

            this.Environment = hosting.EnvironmentName.IfNullOrWhiteSpace("production").ToLower();
            this.IsDevelopmentEnvironment = hosting.IsDevelopment();
            this.RuntimeFramework = Assembly.GetEntryAssembly().GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName;
            //this.ApplicationBasePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
            this.ApplicationBasePath = SherlockUtility.GetApplicationDirectory();

            _instanceIdProvider = instanceIdProvider;
        }

        public string Environment { get; set; }

        public string ApplicationBasePath { get; }

        public string ApplicationInstanceId
        {
            get { return _instanceIdProvider.GetInstanceId(); }
        }

        public bool IsDevelopmentEnvironment { get; }

        public String RuntimeFramework { get; }
    }
}
