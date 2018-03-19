using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sherlock.Framework.Environment;
using System;

namespace Sherlock.Framework.Web.Mvc
{
    public class SherlockControllerActivator : DefaultControllerActivator
    {
        public SherlockControllerActivator(ITypeActivatorCache typeActivatorCache)
            : base(typeActivatorCache)
        { }

        public override object Create(ControllerContext context)
        {
            object controller = base.Create(context);
            return controller;
        }
    }
}
