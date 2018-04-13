using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sherlock.Framework.Caching;
using Sherlock.Framework.Environment;
using Sherlock.Framework.Web.Mvc;
using Sherlock.MvcSample.ApiModule.Services;

namespace Sherlock.MvcSample.ApiModule.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : SherlockApiController
    {
        private Lazy<INewsService> _msgServiceLazy = null;

        private Lazy<ICacheManager> _cacheManagerLazy = null;

        public ValuesController()
        {
            _cacheManagerLazy = new Lazy<ICacheManager>(() => WorkContext.Resolve<ICacheManager>());
            _msgServiceLazy = new Lazy<INewsService>(() => WorkContext.Resolve<INewsService>());


        }


        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet("Test")]
        public async Task<object> TestAsync()
        {
            var url = "https://news.bitcoinworld.com/a/4242";
            var res = await _msgServiceLazy.Value.IsExistsByNewsUrlAsync(url);
            return res;
        }

        [HttpGet("redis")]
        public async Task<object> RedisAsync()
        {
            _cacheManagerLazy.Value.Set($"Test:Num:{1}", 1, TimeSpan.FromMinutes(10));
            var list = _cacheManagerLazy.Value.Get<int>($"Test:Num:{1}");
            return list;
        }

    }
}
