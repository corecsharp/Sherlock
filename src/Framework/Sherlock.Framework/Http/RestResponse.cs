using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sherlock.Framework.Http
{
    public class RestResponse
    {
        public HttpStatusCode StatusCode { get; }

        public string Body { get; }

        public RestResponse(HttpStatusCode statusCode, string body)
        {
            this.StatusCode = statusCode;
            this.Body = body;
        }
    }
}
