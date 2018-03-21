using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Sherlock.Framework.Environment;
using Sherlock.Framework.Environment.ShellBuilders;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Sherlock.Framework.Web.Mvc
{
    public class SherlockControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            ShellBlueprint blue = SherlockEngine.Current.GetRequiredService<ShellBlueprint>();

            foreach (var c in blue.Controllers)
            {
                feature.Controllers.Add(c.Type.GetTypeInfo());
            }
        }
    }
}
