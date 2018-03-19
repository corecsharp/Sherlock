using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sherlock.Framework.Http
{
    public class SherlockRestTimeoutException : SherlockRestException
    {
        public SherlockRestTimeoutException(string message)
            : base(HttpStatusCode.RequestTimeout, message)
        {

        }

        public SherlockRestTimeoutException(string message, Exception innerException)
            : base(HttpStatusCode.RequestTimeout, message, innerException)
        {

        }
    }
}
