using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.Configuration
{
    public class JsonContentConfigurationSource : IConfigurationSource
    {
        private string _jsonString;
        public JsonContentConfigurationSource(string jsonString)
        {
            _jsonString = jsonString;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new JsonContentConfigurationProvider(_jsonString);
        }
    }
}
