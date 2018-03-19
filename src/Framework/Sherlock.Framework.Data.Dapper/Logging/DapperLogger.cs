using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sherlock.Framework.Environment;
using Microsoft.Extensions.Options;
using System.Net;
using Sherlock.Framework.Data;
using Sherlock.Framework.Services;

namespace Sherlock.Framework.Logging
{
    public class DapperLogger : LoggerBase
    {
        private IWorkContextAccessor _workContextAccessor = null;
        private IOptions<SherlockOptions> _SherlockOptions = null;
        private IIdGenerationService _idGenerationService = null;

        public DapperLogger(string name, 
            IIdGenerationService idGenerationService,
            IOptions<SherlockOptions> SherlockOptions,
            IWorkContextAccessor workContextAccessor, 
            Func<string, LogLevel, bool> filter) : base(name, filter)
        {
            Guard.ArgumentNotNull(SherlockOptions, nameof(SherlockOptions));
            Guard.ArgumentNotNull(idGenerationService, nameof(idGenerationService));
            Guard.ArgumentNotNull(workContextAccessor, nameof(workContextAccessor));
            
            _SherlockOptions = SherlockOptions;
            _workContextAccessor = workContextAccessor;
            _idGenerationService = idGenerationService;
        }

        protected override void WriteLog(string name, EventId eventId, LogLevel level, string message, IEnumerable<KeyValuePair<string, object>> extensions)
        {
            LogData data = new LogData();
            data.Id = _idGenerationService.GenerateId();
            data.EventId = eventId.Id;
            data.Message = message;
            data.Category = name.IfNullOrWhiteSpace("NoneCategory");
            data.Level = level;
            data.Application = _SherlockOptions.Value.AppSystemName;
            data.AppVersion = _SherlockOptions.Value.Version;
            data.Host = extensions.FirstOrDefault(kp => kp.Key.CaseInsensitiveEquals("Host")).Value?.ToString();
            data.User = extensions.FirstOrDefault(kp => kp.Key.CaseInsensitiveEquals("User")).Value?.ToString();
            data.TimeCreated = DateTime.UtcNow;
            if (data.Host.IsNullOrWhiteSpace())
            {
                data.Host = SherlockUtility.GetCurrentIPAddress();
            }
            IRepository<LogData> repository = _workContextAccessor.GetContext().ResolveRequired<IRepository<LogData>>();
            repository.Insert(data);
        }
    }
}
