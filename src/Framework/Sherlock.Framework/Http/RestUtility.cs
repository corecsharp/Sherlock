using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sherlock.Framework.Http
{
    public static class RestUtility
    {
        public static Uri BuildUri(Uri baseUri, Version requestedApiVersion, string path, String queryString)
        {
            if (baseUri == null)
            {
                throw new ArgumentNullException(nameof(baseUri));
            }

            var builder = new UriBuilder(baseUri);

            if (requestedApiVersion != null)
            {
                builder.Path += $"v{requestedApiVersion}/";
            }

            if (!string.IsNullOrEmpty(path))
            {
                builder.Path += path;
            }

            if (queryString != null)
            {
                builder.Query = queryString;
            }

            return builder.Uri;
        }
    }
}
