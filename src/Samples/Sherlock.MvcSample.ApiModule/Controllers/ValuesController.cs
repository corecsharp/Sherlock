using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sherlock.Framework.Environment;
using Sherlock.Framework.Web.Mvc;
using Sherlock.MvcSample.ApiModule.Services;

namespace Sherlock.MvcSample.ApiModule.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : SherlockApiController
    {
        private Lazy<INewsService> _msgServiceLazy = null;

        public ValuesController()
        {

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
            var res =await _msgServiceLazy.Value.IsExistsByNewsUrlAsync(url);
            return res;
        }

    }
}
