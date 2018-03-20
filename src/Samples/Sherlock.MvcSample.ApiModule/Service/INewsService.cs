﻿using Sherlock.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.MvcSample.Service
{
    public interface INewsService : IDependency
    {
        Task<bool> IsExistsByNewsUrlAsync(string newsUrl);
    }
}
