using Microsoft.Extensions.Options;
using Sherlock.Framework.Environment;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.Web
{
    public class TimeZoneStateProvider : IWorkContextStateProvider
    {
        private IOptions<SherlockOptions> _options;
        public TimeZoneStateProvider(IOptions<SherlockOptions> configOptions)
        {
            Guard.ArgumentNotNull(configOptions, nameof(configOptions));
            _options = configOptions;
        }

        public Func<WorkContext, object> Get(string name)
        {
            if (name == WorkContext.CurrentTimeZoneState)
            {
                return (WorkContext ctx) =>
                {
                    String timeZone = ctx.CurrentUser?.TimeZone;
                    string timeZoneId = timeZone.IfNullOrWhiteSpace(_options.Value.DefaultTimeZone);
                    return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId) ?? TimeZoneInfo.Local;
                };
            }
            return null;
        }
    }
}
