using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sherlock.Framework.Http
{
    public interface IQueryStringConverterFactory
    {
        IQueryStringConverter GetConverterInstance(Type t);
    }
}
