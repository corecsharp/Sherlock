﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

namespace Sherlock.Framework.Data
{
    /// <summary>
    /// 表示一个提供数据库事务的上下文。
    /// </summary>
    public interface IDatabaseContext
    {
        /// <summary>
        /// 开始一个数据库事务。
        /// </summary>
        /// <param name="level">指定数据库事务的隔离级别。</param>
        /// <param name="dbConnectionName">表示 Data 配置节中的连接字符串名称。</param>
        /// <returns></returns>
        IDatabaseTransaction BeginTransaction(IsolationLevel level = IsolationLevel.ReadCommitted, string dbConnectionName = null);
    }
}
