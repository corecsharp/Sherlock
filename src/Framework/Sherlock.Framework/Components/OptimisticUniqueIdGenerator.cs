using Microsoft.Extensions.Options;
using Sherlock.Framework.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Sherlock.Framework.Components
{
    /// <summary>
    /// 乐观并发式唯一ID生成器
    /// </summary>
    [Obsolete("use 'Sherlock.Framework.Services.IIdGenerationService' instead")]
    public sealed class OptimisticUniqueIdGenerator : IUniqueIdGenerator
    {
        private readonly IDistributedOptimisticStoreService optimisticDataStore;

        private readonly IDictionary<string, ScopeState> states = new Dictionary<string, ScopeState>();
        private readonly object statesLock = new object();

        private int _maxWriteAttempts = 25;
        private int _batchSize = 100;
        
        public OptimisticUniqueIdGenerator(IDistributedOptimisticStoreService optimisticDataStore, 
            IOptions<OptimisticUniqueIdOptions> options)
        {
            Guard.ArgumentNotNull(options, nameof(options));
            this.optimisticDataStore = optimisticDataStore;
            this.BatchSize = (options?.Value == null) ? 100 : options.Value.BatchSize;
            optimisticDataStore.FirstCreationData = (options?.Value == null) ? 1.ToString() : options.Value.Seed.ToString();
        }

        /// <summary>
        /// Id 的增量。
        /// </summary>
        public int Increment { get; private set; } = 1;

        /// <summary>
        /// 每次预准备的 ID 数。
        /// 数字越大性能越好，但是返回的ID不能回收，过大的数字会造成ID资源浪费，此数值应尽量等于每秒最高并发需要的ID数量。
        /// 默认为100。
        /// </summary>
        public int BatchSize
        {
            get { return _batchSize; }
            set
            {
                _batchSize = Math.Max(10, value);
            }
        }

        /// <summary>
        /// 当写请求并发时的重试次数。默认为25。
        /// </summary>
        public int MaxWriteAttempts
        {
            get { return _maxWriteAttempts; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", _maxWriteAttempts, "MaxWriteAttempts must be a positive number.");

                _maxWriteAttempts = value;
            }
        }

        public async Task<long> NewIdAsync(string scopeName)
        {
            Guard.ArgumentNullOrWhiteSpaceString(scopeName, "scopeName");
            var state = GetScopeState(scopeName);


            if (state.LastId == state.HighestIdAvailableInBatch)
            {
                lock (state.IdGenerationSyncRoot) //等待锁和加锁进行原子操作同步
                {
                    if (!state.IdGenerationLock.Wait(30 * 1000))
                    {
                        throw new SherlockException("Waiting update id to storage was timeout, other instance is updating or dead lock .");
                    }
                    state.IdGenerationLock.Reset();
                }

                try
                {
                    if (state.LastId == state.HighestIdAvailableInBatch)
                    {
                        await UpdateFromSyncStore(scopeName, state);
                    }
                }
                finally
                {
                    state.IdGenerationLock.Set();
                }
            }
            return Interlocked.Add(ref state.LastId, this.Increment);
        }

        private ScopeState GetScopeState(string scopeName)
        {
            return states.SafeGetValue(
                scopeName,
                statesLock,
                () => new ScopeState());
        }

        private async Task UpdateFromSyncStore(string scopeName, ScopeState state)
        {
            var writesAttempted = 0;

            while (writesAttempted < this.MaxWriteAttempts)
            {
                var data = await optimisticDataStore.GetDataAsync(scopeName);

                long nextId;
                if (!long.TryParse(data, out nextId))
                {
                    throw new UniqueIdGenerationException(string.Format(
                       "The id seed returned from storage for scope '{0}' was corrupt, and could not be parsed as a long. The data returned was: {1}",
                       scopeName,
                       data));
                }

                state.LastId = nextId - this.Increment;
                state.HighestIdAvailableInBatch = nextId - this.Increment + this.BatchSize * this.Increment;
                var firstIdInNextBatch = state.HighestIdAvailableInBatch + this.Increment;

                bool success = await optimisticDataStore.TryWriteDataAsync(scopeName, firstIdInNextBatch.ToString(CultureInfo.InvariantCulture));
                if (success)
                {
                    return;
                }
                Thread.Sleep(100); //100，防止CPU占用过高（让子弹飞一会儿）。
                writesAttempted++;
            }

            throw new UniqueIdGenerationException(string.Format(
                "Failed to update the data store after {0} attempts. This likely represents too much contention against the store. Increase the batch size to a value more appropriate to your generation load.",
                writesAttempted));
        }
    }
}
