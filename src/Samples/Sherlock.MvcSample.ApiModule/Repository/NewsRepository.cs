using Microsoft.Extensions.Logging;
using Sherlock.Framework.Data;
using Sherlock.MvcSample.ApiModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.MvcSample.ApiModule.Repository
{
    public class NewsRepository : DapperRepository<NewsModel>, INewsRepository
    {
        public NewsRepository(DapperContext dapperContext, ILoggerFactory loggerFactory = null) : base(dapperContext, loggerFactory)
        {

        }

        public bool IsExistsByNewsUrl(string newsUrl)
        {
            throw new NotImplementedException();
        }
    }
}
