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
    public class SherlockApplicationModeProvider : DefaultApplicationModelProvider
    {
        private ShellBlueprint _blueprint = null;
        public const string ModuleRouteKeyName = "module";


        public SherlockApplicationModeProvider(
            IOptions<MvcOptions> options,
            ShellBlueprint blueprint)
            : base(options)
        {
            Guard.ArgumentNotNull(blueprint, nameof(blueprint));

            _blueprint = blueprint;
        }
        
        protected override ControllerModel CreateControllerModel(TypeInfo typeInfo)
        {
            var model = base.CreateControllerModel(typeInfo);
            if (model != null)
            {
                ControllerBlueprintItem item = this.FindBlueprintItem(typeInfo);
                if (item != null)
                {
                    model.RouteValues.Add(ModuleRouteKeyName, item.Feature.Descriptor.ModuleName);

                    //foreach (var s in model.Selectors)
                    //{
                    //    s.ActionConstraints.Add(new SherlockConstraintFactory(item.Feature.Descriptor.ModuleName));
                    //}
                }
            }
           
            return model;
        }

        private ControllerBlueprintItem FindBlueprintItem(TypeInfo typeInfo)
        {
            return _blueprint.Controllers.FirstOrDefault(item => item.Type.Equals(typeInfo.AsType()));
        }
    }
}
