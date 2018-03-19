using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.FileSystem
{
    /// <summary>
    /// 表示物理文件存储实现的配置选项。
    /// </summary>
    public class PhysicalFileStorageOptions
    {
        private IFileUrlProvider _fileUrlProvider;
        private IFilePathRouter _router;
        private IEnumerable<String> _scopes;

        public IEnumerable<String> IncludeScopes
        {
            get { return (_scopes ?? (_scopes ?? Enumerable.Empty<String>())); }
            set { _scopes = value; }
        }

        /// <summary>
        /// 获取或设置文件 Url 提供程序。
        /// </summary>
        public IFileUrlProvider UrlProvider
        {
            get { return (_fileUrlProvider ?? DefaultFileUrlProvider.Instance); }
            set { this._fileUrlProvider = value; }
        }

        public IFilePathRouter PathRouter
        {
            get { return (_router ?? new DefaultFilePathRouter(System.IO.Directory.GetCurrentDirectory())); }
            set { this._router = value; }
        }
    }
}
