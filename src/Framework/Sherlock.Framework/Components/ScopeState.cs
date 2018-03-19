using Sherlock.Threading;
using System.Threading;
using System;

namespace Sherlock.Framework.Components
{
    internal class ScopeState : IDisposable
    {
        public ManualResetEventSlim IdGenerationLock = new ManualResetEventSlim(true);
        public object IdGenerationSyncRoot = new object();
        public long LastId;
        public long HighestIdAvailableInBatch;

        public void Dispose()
        {
            if (IdGenerationLock != null)
            {
                IdGenerationLock.Dispose();
                IdGenerationLock = null;
            }
        }
    }
}