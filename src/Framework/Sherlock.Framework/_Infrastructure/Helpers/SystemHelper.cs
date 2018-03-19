using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sherlock.Helpers
{
    public static class SystemHelper
    {
        /// <summary>
        /// 返回当前系统的中国地区时区Id
        /// </summary>
        /// <returns></returns>
        public static string GetChinaTimeZoneIdByCurrentSys()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "China Standard Time" : "Asia/Shanghai";
        }
    }
}
