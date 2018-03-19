using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.FileSystem
{
    public interface IFileStorageProvider
    {
        IFileStorage CreateStorage(string scope = null);
    }
}
