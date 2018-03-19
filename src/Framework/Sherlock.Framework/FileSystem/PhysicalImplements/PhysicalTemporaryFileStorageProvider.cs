using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.FileSystem
{
    public class PhysicalTemporaryFileStorageProvider : ITemporaryFileStorageProvider
    {
        private IFilePathRouter _router;
        private IFileUrlProvider _urlProvider;
        private int _timeout = 0;

        public PhysicalTemporaryFileStorageProvider(IFilePathRouter router, IFileUrlProvider urlProvider, int fileExpired = 30)
        {
            Guard.ArgumentNotNull(router, nameof(router));
            Guard.ArgumentNotNull(urlProvider, nameof(urlProvider));
            _router = router;
            _urlProvider = urlProvider;
            _timeout = fileExpired;
        }

        public ITemporaryFileStorage CreateStorage()
        {
            return new PhysicalTemporaryFileStorage(_timeout, _router, _urlProvider);
        }
    }
}
