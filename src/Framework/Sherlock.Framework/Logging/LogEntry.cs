using Microsoft.Extensions.Logging;
using Sherlock.Framework.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.Logging
{
    internal sealed class LogEntry : Dictionary<String, Object>
    {
        public string Message { get; set; }

        public EventId EventId { get; set; }

        public LogLevel LogLevel { get; set; }

        public int FormatIndent { get; set; } = 6;

        private string GetString(string key)
        {
            object value;
            this.TryGetValue(key, out value);
            return value?.ToString().IfNullOrWhiteSpace(String.Empty);
        }

        public string ApplicationVersion
        {
            get { return GetString(nameof(ApplicationVersion)).IfNullOrWhiteSpace(SherlockEngine.Current.ApplicationVersion); }
            set { this.Set(nameof(ApplicationVersion), value, true); }
        }

        public string ApplicationName
        {
            get { return GetString(nameof(ApplicationName)).IfNullOrWhiteSpace(SherlockEngine.Current.ApplicationName); }
            set { this.Set(nameof(ApplicationName), value, true); }
        }

        public string User
        {
            get { return GetString(nameof(User)); }
            set { this.Set(nameof(User), value, true); }
        }

        public string Host
        {
            get { return GetString(nameof(Host)); }
            set { this.Set(nameof(Host), value, true); }
        }
    }
}
