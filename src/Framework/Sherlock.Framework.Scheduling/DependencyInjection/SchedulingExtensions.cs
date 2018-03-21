using Microsoft.Extensions.DependencyInjection;
using Sherlock.Framework;
using Sherlock.Framework.DependencyInjection;
using Sherlock.Framework.Environment;
using Sherlock.Framework.Scheduling;
using System;

namespace Sherlock
{
    public static class SchedulingExtensions
    {
        private static Guid _module = Guid.NewGuid();

        /// <summary>
        /// 添加任务调度组件。
        /// </summary>
        /// <param name="builder"><see cref="SherlockServicesBuilder"/> 对象。</param>
        /// <param name="setup">用于配置调度组件的方法。</param>
        /// <returns></returns>
        public static SherlockServicesBuilder AddJobScheduling(this SherlockServicesBuilder builder, Action<SchedulingOptions> setup = null)
        {
            if (builder.AddedModules.Add(_module))
            {
                builder.ServiceCollection.AddSmart(ServiceDescriber.Singleton(typeof(IWorkContextProvider), typeof(JobWorkContextProvider), SmartOptions.Append));
            }
            builder.ServiceCollection.AddSmart(ServiceDescriber.Singleton(typeof(ISchedulingServer), typeof(QuartzSchedulingServer), SmartOptions.TryAdd));
            
            var schedulingConfiguration = builder.Configuration.GetSection("Sherlock:Scheduling");
            builder.ServiceCollection.Configure<SchedulingOptions>(schedulingConfiguration);
            if (setup != null)
            {
                builder.ServiceCollection.Configure(setup);
            }
            ShellEvents.EngineStarted -= ShellEvents_OnEngineStarted;
            ShellEvents.EngineStarted += ShellEvents_OnEngineStarted;
            return builder;
        }

        private static void ShellEvents_OnEngineStarted(SherlockOptions options, IServiceProvider serviceProvider)
        {
            var schedulingServer = serviceProvider.GetRequiredService<ISchedulingServer>();
            schedulingServer.ScheduleAsync().GetAwaiter().GetResult();
        }
    }
}
