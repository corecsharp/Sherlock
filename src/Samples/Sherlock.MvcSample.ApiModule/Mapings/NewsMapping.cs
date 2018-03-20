using Sherlock.Framework.Data;
using Sherlock.MvcSample.ApiModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.MvcSample.ApiModule.Mapings
{
    public class NewsMapping:DapperMetadataProvider<NewsModel>
    {

        protected override void ConfigureModel(DapperMetadataBuilder<NewsModel> builder)
        {
            builder.TableName("news");
            builder.HasKey(s => new { s.Id });//主键
        }

    }
}
