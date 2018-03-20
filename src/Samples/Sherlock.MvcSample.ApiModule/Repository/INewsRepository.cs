using Sherlock.Framework;
using Sherlock.Framework.Data;
using Sherlock.MvcSample.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.MvcSample.Repository
{
    public interface INewsRepository : IRepository<NewsModel>, IDependency
    {
        /// <summary>
        /// 通过NewsUrl判断新闻是否存在
        /// </summary>
        /// <param name="newsUrl"></param>
        /// <returns></returns>
        bool IsExistsByNewsUrl(string newsUrl);


    }
}
