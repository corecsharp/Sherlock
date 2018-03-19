using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.Components
{
    /// <summary>
    /// 唯一 Id 生成器选项。
    /// </summary>
    [Obsolete("use 'Sherlock.Framework.Services.IIdGenerationService' instead")]
    public sealed class OptimisticUniqueIdOptions
    {
        private int _seed;
        private int _increment;
        private int _batchSize;

        public OptimisticUniqueIdOptions()
        {
            _seed = 1;
            _increment = 1;
            _batchSize = 20;
        }

        /// <summary>
        /// Id 的种子（起始值, 仅用于第一次初始化，默认为 1，必须大于 0）。
        /// </summary>
        public int Seed
        {
            get { return _seed; }
            set
            {
                if (_seed < 0)
                {
                    throw new ArgumentOutOfRangeException("唯一 id 生成组件的种子不能小于 0。");
                }
                _seed = value;
            }
        }

        /// <summary>
        /// Id 的增量（默认为 1，必须大于 0）。
        /// </summary>
        public int Increment
        {
            get { return _increment; }
            set
            {
                if (_increment <= 0)
                {
                    throw new ArgumentOutOfRangeException("唯一 id 生成组件的增量必须大于 0。");
                }
                _increment = value;
            }
        }

        /// <summary>
        /// 批处理尺寸，每次预准备的 ID 数（默认为 20，不能小于 1）。
        /// 数字越大性能越好，但是返回的ID不能回收，过大的数字会造成ID资源浪费，此数值应尽量等于每秒最高并发需要的ID数量。
        /// 默认为100。
        /// </summary>
        public int BatchSize
        {
            get { return _batchSize; }
            set
            {
                if (_batchSize < 1)
                {
                    throw new ArgumentOutOfRangeException("唯一 id 生成组件的批处理尺寸不能小于 1。");
                }
                _batchSize = value;
            }
        }
    }
}
