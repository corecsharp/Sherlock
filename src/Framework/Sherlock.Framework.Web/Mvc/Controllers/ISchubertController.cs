using Microsoft.Extensions.Logging;
using Sherlock.Framework.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.Web.Mvc
{
    internal interface ISherlockController
    {
        WorkContext WorkContext { get; }
        ILoggerFactory LoggerFactory { get; }
    }
}
