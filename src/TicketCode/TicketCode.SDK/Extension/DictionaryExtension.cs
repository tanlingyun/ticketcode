using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HxSDK.TicketCode.Extensions
{
    internal static class DictionaryExtension
    {
        /// <summary>
        /// 字典转url参数
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="IgnoreEmpty">是否忽略为空的字符</param>
        /// <returns></returns>
        public static string ToUrl<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, bool IgnoreEmpty = true)
        {
            return string.Join("&", dictionary.Select(x => $"{x.Key}={x.Value}"));
        }
    }
}
