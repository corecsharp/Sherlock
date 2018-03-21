using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.Threading
{
   
    /// <summary>
    ///  实现一次调度程序
    /// </summary>
    public interface ITimeoutTimer : IDisposable
    {

        /// <summary>
        /// 在 <paramref name="delay"/> 置顶的延时之后执行一次性调度任务。
        /// </summary>
        /// <param name="task">要执行的任务。</param>
        /// <param name="delay">延迟时间。</param>
        /// <returns>一个  <see cref="ITimeout"/> 对象。</returns>
        ITimeout NewTimeout(ITimerTask task, TimeSpan delay);
        
        /// <summary>
        /// 取消调度器中已经添加排队但没有执行的任务。
        /// </summary>
        /// <returns></returns>
        IEnumerable<ITimeout> Stop();
    }

    public static class ITimeoutTimerExtensions
    {
        /// <summary>
        /// 在 <paramref name="delay"/> 置顶的延时之后执行一次性调度任务。
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="action">要执行的任务。</param>
        /// <param name="delay">延迟时间。</param>
        /// <returns><see cref="ITimeout"/> 对象。</returns>
        public static ITimeout NewTimeout(this ITimeoutTimer timer, Action<ITimeout> action, TimeSpan delay)
        {
            Guard.ArgumentNotNull(action, nameof(action));
            return timer.NewTimeout(new DelegateTask(action), delay);
        }
        private class DelegateTask : ITimerTask
        {
            private Action<ITimeout> _delegate;

            public DelegateTask(Action<ITimeout> @delegate)
            {
                _delegate = @delegate;
            }

            public void Run(ITimeout timeout)
            {
                _delegate.Invoke(timeout);
            }
        }
    }
}
