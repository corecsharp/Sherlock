using Sherlock.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.Logging
{
    public class LogDataMetadataProvier : DapperMetadataProvider<LogData>
    {
        private string _tableName = null;

        public LogDataMetadataProvier(string tableName)
        {
            _tableName.IfNullOrWhiteSpace("Logs");
        }

        protected override void ConfigureModel(DapperMetadataBuilder<LogData> builder)
        {
            builder.HasKey(lg => lg.Id);
            builder.TableName(_tableName);
        }
    }
}
