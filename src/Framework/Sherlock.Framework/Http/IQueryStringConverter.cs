﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sherlock.Framework.Http
{
    public interface IQueryStringConverter
    {
        bool CanConvert(Type t);

        string[] Convert(object o);
    }
}
