using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.FileSystem
{
    public static class PhysicalFileStorageExtensions
    {
        /// <summary>
        /// 以指定的配置添加物理文件存储器。
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="options">物理文件存储配置。</param>
        public static void AddPhysicalFileStorage(this IFileStorageManager manager, PhysicalFileStorageOptions options)
        {
            options = options ?? new PhysicalFileStorageOptions();
            PhysicalFileStorageProvider provider = new PhysicalFileStorageProvider(options);
            manager.AddProvider(provider);
        }

        /// <summary>
        /// 为指定的范围添加物理文件存储器。
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="scopes">支持的存储范围。</param>
        public static void AddPhysicalFileStorage(this IFileStorageManager manager, params string[] scopes)
        {
            var options = new PhysicalFileStorageOptions() { IncludeScopes = scopes };
            PhysicalFileStorageProvider provider = new PhysicalFileStorageProvider(options);
            manager.AddProvider(provider);
        }

        /// <summary>
        /// 以指定的根目录为特定范围添加物理文件存储器。
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="basePath">物理文件存储的根目录。</param>
        /// <param name="scopes">文件存储范围。</param>
        public static void AddPhysicalFileStorage(this IFileStorageManager manager, string basePath, params string[] scopes)
        {
            Guard.AbsolutePhysicalPath(basePath, nameof(basePath));
            var options = new PhysicalFileStorageOptions() { IncludeScopes = scopes, PathRouter = new DefaultFilePathRouter(basePath) };
            PhysicalFileStorageProvider provider = new PhysicalFileStorageProvider(options);
            manager.AddProvider(provider);
        }

        /// <summary>
        /// 以指定的根目录和 URL 映射程序为特定范围添加物理文件存储器。
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="basePath">物理文件存储的根目录。</param>
        /// <param name="urlProvider">URL 映射程序。</param>
        /// <param name="scopes">文件存储范围。</param>
        public static void AddPhysicalFileStorage(this IFileStorageManager manager, string basePath, IFileUrlProvider urlProvider, params string[] scopes)
        {
            Guard.AbsolutePhysicalPath(basePath, nameof(basePath));
            var options = new PhysicalFileStorageOptions() { IncludeScopes = scopes, UrlProvider = urlProvider, PathRouter = new DefaultFilePathRouter(basePath) };
            PhysicalFileStorageProvider provider = new PhysicalFileStorageProvider(options);
            manager.AddProvider(provider);
        }

        /// <summary>
        /// 使用物理文件存储器用作临时文件存储。
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="urlProvider">临时文件指定的 Url 生成提供程序。</param>
        /// <param name="pathRouter">临时文件路径路由器。</param>
        /// <param name="tempFileExpiredMinutes">临时文件过期时间（分钟）。</param>
        public static void UsePhysicalTemporaryFileStorage(this IFileStorageManager manager, IFileUrlProvider urlProvider, DefaultFilePathRouter pathRouter, int tempFileExpiredMinutes = 30)
        {
            var provider = new PhysicalTemporaryFileStorageProvider(
                pathRouter ?? DefaultFilePathRouter.Instance, 
                urlProvider ?? DefaultFileUrlProvider.Instance,
                tempFileExpiredMinutes);
            manager.SetTemporaryProvider(provider);
        }

        /// <summary>
        /// 以指定的根目录使用物理文件存储器用作临时文件存储。
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="basePath">临时文件存储的根目录。</param>
        /// <param name="tempFileExpiredMinutes">临时文件过期时间（分钟）。</param>
        public static void UsePhysicalTemporaryFileStorage(this IFileStorageManager manager, string basePath, int tempFileExpiredMinutes = 30)
        {
            Guard.AbsolutePhysicalPath(basePath, basePath);
            manager.UsePhysicalTemporaryFileStorage(null, new DefaultFilePathRouter(basePath), tempFileExpiredMinutes);
        }

        /// <summary>
        /// 以指定的根目录和 URL 映射程序配置物理文件存储器用作临时文件存储。
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="basePath">临时文件存储的根目录。</param>
        /// <param name="urlProvider">文件 URL 映射程序。</param>
        /// <param name="tempFileExpiredMinutes">临时文件过期时间（分钟）。</param>
        public static void UsePhysicalTemporaryFileStorage(this IFileStorageManager manager, string basePath, IFileUrlProvider urlProvider, int tempFileExpiredMinutes = 30)
        {
            Guard.AbsolutePhysicalPath(basePath, basePath);
            manager.UsePhysicalTemporaryFileStorage(urlProvider, new DefaultFilePathRouter(basePath), tempFileExpiredMinutes);
        }
    }
}
