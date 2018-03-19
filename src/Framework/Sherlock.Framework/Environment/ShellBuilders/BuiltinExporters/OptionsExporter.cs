using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sherlock.Framework.Environment.Modules;

namespace Sherlock.Framework.Environment.ShellBuilders.BuiltinExporters
{
    public class OptionsExporter : IShellBlueprintItemExporter
    {
        public string Category { get { return BuiltinBlueprintItemCategories.Options; } }

        public bool CanExport(Type type)
        {
            return type.HasAttribute<ConfiguredOptionsAttribute>();
        }

        public ShellBlueprintItem Export(Type type, Feature feature)
        {
            ConfiguredOptionsAttribute attribute = type.GetAttribute<ConfiguredOptionsAttribute>();
            return new OptionsBlueprintItem { Type = type, Feature = feature, ConfigurationSection = attribute.ConfigurationSection };
        }
    }
}
