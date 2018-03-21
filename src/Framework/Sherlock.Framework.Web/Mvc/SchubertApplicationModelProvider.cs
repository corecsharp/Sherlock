using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.Options;
using Sherlock.Framework.Environment.ShellBuilders;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sherlock.Framework.Web.Mvc
{

    public class SherlockApplicationModeProvider : IApplicationModelProvider
    {
        private ShellBlueprint _blueprint = null;
        public const string ModuleRouteKeyName = "module";

        public int Order => 0;

        public SherlockApplicationModeProvider(ShellBlueprint blueprint)
        {
            Guard.ArgumentNotNull(blueprint, nameof(blueprint));
            
            _blueprint = blueprint;
        }

        private ControllerBlueprintItem FindBlueprintItem(TypeInfo typeInfo)
        {
            return _blueprint.Controllers.FirstOrDefault(item => item.Type.Equals(typeInfo.AsType()));
        }

        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            foreach (var controller in context.Result.Controllers)
            {
                ControllerBlueprintItem item = this.FindBlueprintItem(controller.ControllerType);
                if (item != null)
                {
                    controller.RouteValues.Add(ModuleRouteKeyName, item.Feature.Descriptor.ModuleName);
                }
            }
        }

        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
            
        }
    }
}
