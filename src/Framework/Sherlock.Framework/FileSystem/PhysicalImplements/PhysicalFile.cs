using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.FileSystem
{
    public class PhysicalFile : IFile
    {
        private string _fullPath = null;
        private IFileUrlProvider _fileUrlProvider = null;
        private string _url = null;
        private string _filePath = null;
        private string _fileName = null;
        public PhysicalFile(string scope, string filePath, IFilePathRouter router, IFileUrlProvider urlProvider)
        {
            Guard.ArgumentNotNull(urlProvider, nameof(urlProvider));
            Guard.ArgumentIsRelativePath(filePath, nameof(filePath));
            _filePath = filePath;
            _fullPath = router.GetFilePath(filePath, scope);
            _fileUrlProvider = urlProvider;
        }

        public bool Exists
        {
            get { return !this._fullPath.IsNullOrWhiteSpace() && File.Exists(this._fullPath); }
        }

        public string FullPath
        {
            get
            {
                return _filePath;
            }
        }

        public DateTime LastModifiedTimeUtc
        {
            get
            {
                return File.GetLastWriteTimeUtc(_fullPath);
            }
        }

        public long Length
        {
            get
            {
                FileInfo file = new FileInfo(_fullPath);
                return file.Length;
            }
        }

        public string Name
        {
            get
            {
                return (_fileName ?? (_fileName = Path.GetFileName(_fullPath)));
            }
        }

        public string CreateAccessUrl()
        {
            return _url ?? (_url = _fileUrlProvider.CreateAccessUrl(_fullPath));
        }

        public Stream CreateReadStream()
        {
            return File.OpenRead(_fullPath);
        }

        public Task<Stream> CreateReadStreamAsync()
        {
            return Task.FromResult<Stream>(File.OpenRead(_fullPath));
        }
    }
}
