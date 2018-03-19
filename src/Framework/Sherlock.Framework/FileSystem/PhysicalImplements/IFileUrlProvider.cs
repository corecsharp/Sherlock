using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.FileSystem
{
    /// <summary>
    /// 表示物理访问路径提供程序。
    /// </summary>
    public interface IFileUrlProvider
    {
        /// <summary>
        /// 根据文件完整路径获取访问 Url。
        /// </summary>
        /// <param name="physicalPath">物理文件完整访问路径。</param>
        /// <returns>可访问地址。</returns>
        string CreateAccessUrl(string physicalPath);
    }

    public class DefaultFileUrlProvider : IFileUrlProvider
    {
        public string CreateAccessUrl(string fileFullPath)
        {
            Guard.AbsolutePhysicalPath(fileFullPath, nameof(fileFullPath));
            return $"file:///{fileFullPath}";
        }

        public static readonly DefaultFileUrlProvider _instance = new DefaultFileUrlProvider();

        public static DefaultFileUrlProvider Instance { get { return _instance; } }
    }
}
