using System;
using System.Collections.Generic;
using System.Text;

namespace HxSDK.TicketCode.Extensions
{
    internal static class DateTimeExtension
    {
        /// <summary>
        /// 时间戳(时间戳是指格林威治时间1970年01月01日00时00分00秒起至现在的总毫秒数)
        /// </summary>
        /// <param name="time"></param>
        /// <param name="kind">0-总秒数，1-总毫秒数</param>
        /// <returns></returns>
        public static long Timestamp(this DateTime time, int kind = 0)
        {
            if (kind == 1)
                return (long)time.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            else
                return (int)time.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}
