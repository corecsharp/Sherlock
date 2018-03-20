using Sherlock.Framework.Data;
using Sherlock.MvcSample.ApiModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.MvcSample.ApiModule.Services
{
    public class NewsService : INewsService
    {
        private IRepository<NewsModel> _repository;


        public NewsService(IRepository<NewsModel> repository) {
            _repository = repository;
        }

        public async Task<bool> IsExistsByNewsUrlAsync(string newsUrl)
        {
            var filter = new SingleQueryFilter();
            filter.AddEqual(nameof(NewsModel.NewsUrl), newsUrl);
            NewsModel entity = await _repository.QueryFirstOrDefaultAsync(filter);
            return entity != null;
        }
    }
}
