using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Controllers;
using Sherlock.Framework.Environment;
using Sherlock.Framework.Services;
using Sherlock.Framework.Web;
using Sherlock.Framework.Web.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Sherlock.Framework.Environment.Modules.Finders;
using Sherlock.Framework.Environment.ShellBuilders;
using Sherlock.Framework.Environment.ShellBuilders.BuiltinExporters;

namespace Sherlock.Framework.DependencyInjection
{
    public static class SherlockWebServices
    {
        public static IEnumerable<SmartServiceDescriptor> GetServices(SherlockWebOptions options)
        {
            Guard.ArgumentNotNull(options, nameof(options));

            yield return ServiceDescriber.Transient<IModuleFinder, PackageFinder>(SmartOptions.Append);

            yield return ServiceDescriber.Singleton<IHttpContextAccessor, HttpContextAccessor>();
            yield return ServiceDescriber.Scoped<HttpWorkContext, HttpWorkContext>();
            yield return ServiceDescriber.Singleton<IWorkContextProvider, HttpWorkContextProvider>(SmartOptions.Append);
            yield return ServiceDescriber.Transient<ISherlockEnvironment, AspNetEnvironment>();
            yield return ServiceDescriber.Transient<ICookiesAccessor, CookiesAccessor>();
            
            yield return ServiceDescriber.Scoped<IClientEnvironment, ClientEnvironment>();
            
            yield return ServiceDescriber.Transient<IShellBlueprintItemExporter, ControllerExporter>(SmartOptions.Append);

            if (options.MvcFeatures != MvcFeatures.None)
            {
                yield return ServiceDescriber.Transient<IApplicationModelProvider, SherlockApplicationModeProvider>(SmartOptions.Replace);
                yield return ServiceDescriber.Transient<IControllerActivator, SherlockControllerActivator>(SmartOptions.Replace);

                if (options.MvcFeatures == MvcFeatures.Full)
                {
                    yield return ServiceDescriber.Scoped<IHtmlSegmentManager, HtmlSegmentManager>();
                }
                
            }
        }
    }
}
