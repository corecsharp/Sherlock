using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sherlock.Framework.Http
{
    public class SherlockRestException : SherlockException
    {
        public HttpStatusCode HttpCode { get; set; }

        public SherlockRestException(HttpStatusCode code)
        {
        }
        public SherlockRestException(HttpStatusCode code, string message)
            : base(message)
        {
            this.HttpCode = code;
        }
        
        public SherlockRestException(HttpStatusCode code, string message, Exception innerException)
            : base(message, innerException)
        {
            this.HttpCode = code;
        }
    }
}
