using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace TicketCode.Infrastructure.Extensions
{
    public static class HttpRequestExtension
    {
        /// <summary>
        /// 获取用户浏览器UserAgent
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string UserAgent(this HttpRequest request)
        {
            return request.Headers.ContainsKey("User-Agent") ? request.Headers["User-Agent"][0] : "";
        }
    }
}
