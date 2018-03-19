using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sherlock.Framework.Json
{
    public class LongToStringContract : JsonPrimitiveContract
    {
        private static WeakReference<LongToStringJsonConverter> _longToString;

        private static LongToStringJsonConverter GetLongToStringConverter()
        {
            if (_longToString == null)
            {
                LongToStringJsonConverter contract = new LongToStringJsonConverter();
                _longToString = new WeakReference<LongToStringJsonConverter>(contract);
            }
            if (_longToString.TryGetTarget(out LongToStringJsonConverter c))
            {
                return c;
            }
            _longToString = null;
            return GetLongToStringConverter();
        }

        public LongToStringContract(Type underlyingType) : base(underlyingType)
        {
            this.Converter = GetLongToStringConverter();
        }

        
    }
}
