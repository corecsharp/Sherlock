﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.FileSystem
{
    /// <summary>
    /// 文件存储工厂接口。
    /// </summary>
    public interface IFileStorageManager
    {
        /// <summary>
        /// 在指定范围创建文件存储对象。
        /// </summary>
        /// <param name="scope">范围。</param>
        /// <returns><see cref="IFileStorage"/> 实例。</returns>
        IFileStorage CreateStorage(string scope = null);

        /// <summary>
        /// 创建临时文件存储对象。
        /// </summary>
        /// <returns><see cref="ITemporaryFileStorage"/> 实例。</returns>
        ITemporaryFileStorage CreateTemporaryStorage();

        /// <summary>
        /// 获取或设置临时文件存储提供程序。
        /// </summary>
        [Obsolete("Temporary 属性包含歧义，请使用 CreateTemporaryStorage 方法代替。")]
        ITemporaryFileStorage Temporary { get; }

        /// <summary>
        /// 添加文件存储提供程序。
        /// </summary>
        /// <param name="provider">文件存储提供程序。</param>
        void AddProvider(IFileStorageProvider provider);

        void SetTemporaryProvider(ITemporaryFileStorageProvider provider);
    }
}
