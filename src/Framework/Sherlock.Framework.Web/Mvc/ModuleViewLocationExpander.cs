using Microsoft.AspNetCore.Mvc.Razor;
using Sherlock.Framework.Environment;
using Sherlock.Framework.Environment.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.Web.Mvc
{
    public class ModuleViewLocationExpander : IViewLocationExpander
    {

        public IEnumerable<string> ExpandViewLocations(
            ViewLocationExpanderContext context,
            IEnumerable<string> viewLocations)
        {
            if (context.Values.ContainsKey(SherlockApplicationModeProvider.ModuleRouteKeyName))
            {
                var moduleName = RazorViewEngine.GetNormalizedRouteValue(context.ActionContext, SherlockApplicationModeProvider.ModuleRouteKeyName);

                viewLocations = viewLocations.Select(lo => $"/Modules/{moduleName}/{lo.TrimStart('/')}");
                viewLocations = viewLocations.Union(new string[] { "/Views/Shared/{0}.cshtml" }).ToArray();
            }
            return viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            var moduleMeatadata = context.ActionContext.ActionDescriptor.RouteValues.FirstOrDefault(
                s => s.Key.CaseInsensitiveEquals(SherlockApplicationModeProvider.ModuleRouteKeyName) && !(s.Value?.ToString()).IsNullOrWhiteSpace());

            if (!moduleMeatadata.Key.IsNullOrWhiteSpace())
            {
                context.Values[SherlockApplicationModeProvider.ModuleRouteKeyName] = moduleMeatadata.Value.ToString();
            }
        }

    }
}
