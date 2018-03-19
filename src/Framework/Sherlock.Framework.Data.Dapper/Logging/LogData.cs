using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.Logging
{
    public class LogData
    {
        public long Id { get; set; }

        public string Application { get; set; }

        public string Category { get; set; }

        public int EventId { get; set; }

        public string Message { get; set; }

        public LogLevel Level { get; set; }

        public string Host { get; set; }

        public string User { get; set; }

        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;

        public string AppVersion { get; set; }
    }
}
