using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TicketCode.Core.Redis
{
    public interface IRedisCache
    {
        /// <summary>
        /// 像列表右侧插入数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        void ListRightPush(string key, string[] values);

        /// <summary>
        /// 像列表右侧插入数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task ListRightPushAsync(string key, string[] values, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// 从列表最前面弹出元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string ListLeftPop(string key);

        Task<string> ListLeftPopAsync(string key, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// 获取列表长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long ListLength(string key);

        /// <summary>
        /// 获取列表长度
        /// </summary>
        /// <param name="key"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<long> ListLengthAsync(string key, CancellationToken token = default(CancellationToken));
    }
}
