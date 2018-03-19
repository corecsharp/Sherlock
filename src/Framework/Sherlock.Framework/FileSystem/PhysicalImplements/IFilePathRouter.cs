using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.FileSystem
{
    /// <summary>
    /// 表示一个文件路径路由。
    /// </summary>
    public interface IFilePathRouter
    {
        /// <summary>
        /// 将应用程序相对路径（例如 AAA/bbb.jpg） 映射到物理绝对路径（例如 C:\AAA\bbb.jpg）
        /// </summary>
        /// <param name="relativePath">要映射的应用程序相对路径。</param>
        /// <param name="scope">存储区域。</param>
        /// <returns></returns>
        string GetFilePath(string relativePath, string scope);

        /// <summary>
        /// 将物理据对路径（例如 C:\AAA\bbb.jpg）映射为应用程序相对路径（例如 AAA/bbb.jpg） 
        /// </summary>
        /// <param name="physicalPath">要映射的物理路径。</param>
        /// <param name="scope">存储区域。</param>
        /// <returns></returns>
        string GetRelativeApplicationPath(string physicalPath, string scope);
    }

    public class DefaultFilePathRouter : IFilePathRouter
    {
        private string _basePath;
        private static readonly DefaultFilePathRouter _instance = new DefaultFilePathRouter(Directory.GetCurrentDirectory());
        public DefaultFilePathRouter(string basePath)
        {
            Guard.AbsolutePhysicalPath(basePath, nameof(basePath));
            _basePath = basePath;
        }

        public static DefaultFilePathRouter Instance { get { return _instance; } }

        public string GetFilePath(string relativePath, string scope)
        {
            if (scope.IsNullOrWhiteSpace())
            {
                return Path.Combine(_basePath, relativePath.Replace("/", "\\").TrimStart('\\'));
            }
            return Path.Combine(_basePath.TrimEnd('\\'), scope, relativePath.Replace("/", "\\").TrimStart('\\'));
        }

        public string GetRelativeApplicationPath(string physicalPath, string scope)
        {
            Guard.AbsolutePhysicalPath(physicalPath, nameof(physicalPath));
            string root = _basePath;
            if (!scope.IsNullOrWhiteSpace())
            {
                root = Path.Combine(_basePath, scope);
            }
            return physicalPath.Replace(root, String.Empty).TrimStart('\\').Replace("\\", "/");
        }
    }
}
