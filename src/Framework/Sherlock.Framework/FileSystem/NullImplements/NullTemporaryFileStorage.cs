using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.FileSystem
{
    internal sealed class NullTemporaryFileStorage : NullFileStorage, ITemporaryFileStorage
    {
        public Task ClearAsync()
        {
            return Task.FromResult(0);
        }
    }
}
