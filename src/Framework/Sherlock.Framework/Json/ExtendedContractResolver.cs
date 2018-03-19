using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sherlock.Framework.Json
{
    public class ExtendedCamelCaseContractResolver : CamelCasePropertyNamesContractResolver
    {
        private bool longAsString = false;
        /// <summary>
        /// 创建 <see cref="ExtendedCamelCaseContractResolver"/> 的新实例。
        /// </summary>
        /// <param name="useLongAsString">是否将 long 序列化为 string 类型（javascript 无法使用 64 位整数）。</param>
        public ExtendedCamelCaseContractResolver(bool useLongAsString = false) :base()
        {
            this.longAsString = useLongAsString;
        }

        private static bool IsLongOrNullableLong(Type objectType)
        {
            return (objectType.Equals(typeof(long)) || objectType.Equals(typeof(long?)));
        }

        protected override JsonPrimitiveContract CreatePrimitiveContract(Type objectType)
        {
            if (IsLongOrNullableLong(objectType) && longAsString)
            {
                return new LongToStringContract(objectType);
            }
            return base.CreatePrimitiveContract(objectType);
        }

    }

    public class ExtendedContractResolver : DefaultContractResolver
    {
        private bool longAsString = false;
        /// <summary>
        /// 创建 <see cref="ExtendedContractResolver"/> 的新实例。
        /// </summary>
        /// <param name="useLongAsString">是否将 long 序列化为 string 类型（javascript 无法使用 64 位整数）。</param>
        public ExtendedContractResolver(bool useLongAsString = false) :base()
        {
            this.longAsString = useLongAsString;
        }

        private static bool IsLongOrNullableLong(Type objectType)
        {
            return (objectType.Equals(typeof(long)) || objectType.Equals(typeof(long?)));
        }

        protected override JsonPrimitiveContract CreatePrimitiveContract(Type objectType)
        {
            if (IsLongOrNullableLong(objectType) && longAsString)
            {
                return new LongToStringContract(objectType);
            }
            return base.CreatePrimitiveContract(objectType);
        }
    }
}
