using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sherlock.Framework.Environment;
using Sherlock.Framework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.Logging
{
    public class DapperLoggerProvider : ILoggerProvider
    {
        private IOptions<SherlockOptions> _SherlockOptions = null;
        private IWorkContextAccessor _workContextAccessor = null;
        private Func<string, LogLevel, bool> _filter = null;
        private IIdGenerationService _idGenerationService = null;

        public DapperLoggerProvider(
            IIdGenerationService idGenerationService,
            IWorkContextAccessor workContextAccessor,
            IOptions<SherlockOptions> SherlockOptions,
            Func<string, LogLevel, bool> filter)
        {
            Guard.ArgumentNotNull(_idGenerationService, nameof(_idGenerationService));
            Guard.ArgumentNotNull(workContextAccessor, nameof(workContextAccessor));
            Guard.ArgumentNotNull(SherlockOptions, nameof(SherlockOptions));
            Guard.ArgumentNotNull(filter, nameof(filter));

            _filter = filter;
            _idGenerationService = idGenerationService;
            _workContextAccessor = workContextAccessor;
            _SherlockOptions = SherlockOptions;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new DapperLogger(categoryName, _idGenerationService, _SherlockOptions, _workContextAccessor, _filter);
        }

        public void Dispose()
        {
            
        }
    }
}
