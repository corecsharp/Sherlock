using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Sherlock.Framework.Web.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework
{
    public static class DataProtectionConfigurationExtensions
    {
        /// <summary>
        /// 使用 Sherlock 的文件存储（<see cref="Sherlock.Framework.FileSystem.IFileStorage"/>）系统来存储应用程序的键。
        /// </summary>
        /// <param name="dpc"></param>
        /// <param name="directoryPath">用于键存储的根目录。</param>
        /// <param name="scope">存储分区。</param>
        /// <returns></returns>
        public static IDataProtectionBuilder PersistKeysToFileStorage(this IDataProtectionBuilder dpc, string directoryPath = null, string scope = null)
        {
            Guard.ArgumentIsRelativePath(directoryPath, nameof(directoryPath));

            var repository = ServiceDescriptor.Singleton<IXmlRepository>(services => new FileStorageXmlRepository(services, scope, directoryPath));
            Use(dpc.Services, repository);
            return dpc;
        }

        private static void Use(IServiceCollection services, ServiceDescriptor descriptor)
        {
            // We go backward since we're modifying the collection in-place.
            for (int i = services.Count - 1; i >= 0; i--)
            {
                if (services[i]?.ServiceType == descriptor.ServiceType)
                {
                    services.RemoveAt(i);
                }
            }
            services.Add(descriptor);
        }

    }
}
