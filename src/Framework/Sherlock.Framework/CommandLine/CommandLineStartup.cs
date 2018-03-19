using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sherlock.Framework.DependencyInjection;
using System;

namespace Sherlock.Framework
{
    public abstract class CommandLineStartup
    {
        protected CommandLineStartup()
        {

        }

        public IConfigurationRoot Configuration { get; protected internal set; }

        protected internal abstract void BuildConfiguration(String environment, IConfigurationBuilder builder);

        protected internal abstract void ConfigureServices(SherlockServicesBuilder builder);

        protected internal abstract void Configure(IServiceProvider serviceProvider);

        protected internal virtual void ConfigureShellCreationScope(ShellCreationScope scope)
        { }
    }
}
