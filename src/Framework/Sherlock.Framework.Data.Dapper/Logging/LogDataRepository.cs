using Sherlock.Framework.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Data;
using Sherlock.Framework.Data;

namespace Sherlock.Framework.Logging
{
    public class LogDataRepository : DapperRepository<LogData>
    {
        public LogDataRepository(string connectionStringName, DapperContext dapperContext, ILoggerFactory loggerFactory = null) : base(dapperContext, loggerFactory)
        {
            this.ConnectionStringName = connectionStringName;
        }

        public string ConnectionStringName { get; }

        protected override IDbConnection GetReadingConnection()
        {
            return this.Context.GetConnection(this.ConnectionStringName);
        }

        protected override IDbConnection GetWritingConnection()
        {
            return this.Context.GetConnection(this.ConnectionStringName);
        }
    }
}
