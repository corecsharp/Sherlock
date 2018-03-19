using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sherlock.Framework.Environment.Modules;
using System.Reflection;

namespace Sherlock.Framework.Environment.ShellBuilders.BuiltinExporters
{
    public class DependencyExporter : IShellBlueprintItemExporter
    {
        public string Category
        {
            get { return BuiltinBlueprintItemCategories.Dependency; }
        }

        public bool CanExport(Type type)
        {
            return typeof(IDependency).GetTypeInfo().IsAssignableFrom(type);
        }

        public ShellBlueprintItem Export(Type type, Feature feature)
        {
            return new ShellBlueprintItem { Type = type, Feature = feature };
        }
    }
}
