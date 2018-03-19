using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Sherlock.Framework.Http
{
    public class RestStreamedResponse
    {
        public HttpStatusCode StatusCode { get; }

        public Stream Body { get; }

        public HttpResponseHeaders Headers { get; }

        public RestStreamedResponse(HttpStatusCode statusCode, Stream body, HttpResponseHeaders headers)
        {
            this.StatusCode = statusCode;
            this.Body = body;
            this.Headers = headers;
        }
    }
}
