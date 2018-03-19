using System;
using System.IO;

namespace Sherlock.Framework.Environment
{
    public class DefaultPathProvider : IPathProvider
    {
        private ISherlockEnvironment _applicationEnvironment;

        private static bool _folderValid = false;
        private readonly static object ValidSync = new object();

        public DefaultPathProvider(ISherlockEnvironment applicationEnvironment)
        {
            Guard.ArgumentNotNull(applicationEnvironment, nameof(applicationEnvironment));
            
            _applicationEnvironment = applicationEnvironment;
        }
        
        private void EnsureFolderExsited()
        {
            if (!_folderValid)
            {
                lock (ValidSync)
                {
                    if (!_folderValid)
                    {
                        if (!Directory.Exists(RootDirectoryPhysicalPath))
                        {
                            Directory.CreateDirectory(RootDirectoryPhysicalPath);
                        }
                        _folderValid = true;
                    }
                }
            }
        }

        public virtual string RootDirectoryPhysicalPath
        {
            get
            {
                return _applicationEnvironment.ApplicationBasePath;
            }
        }

        public string MapApplicationPath(string virtualPath)
        {

            this.EnsureFolderExsited();
            if (String.IsNullOrWhiteSpace(virtualPath))
            {
                return this.RootDirectoryPhysicalPath;
            }

            string subPath = virtualPath;
            if (virtualPath.StartsWith("~/"))
            {
                subPath = virtualPath.Substring(2);
            }
            if (virtualPath.StartsWith("/"))
            {
                subPath = virtualPath.Substring(1);
            }
            subPath.Replace('/', '\\');
            return Path.Combine(this.RootDirectoryPhysicalPath, subPath);
        }
    }
}