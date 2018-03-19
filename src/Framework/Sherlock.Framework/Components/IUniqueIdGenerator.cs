using System.Threading.Tasks;
using Sherlock.Framework.Services;
using System;

namespace Sherlock.Framework.Components
{
    /// <summary>
    /// 唯一 Id 生成器。
    /// </summary>
    [Obsolete("use 'Sherlock.Framework.Services.IIdGenerationService' instead")]
    public interface IUniqueIdGenerator
    {
        Task<long> NewIdAsync(string scopeName);
    }
}