using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCode.Core.Models;
using TicketCode.Core.Redis;
using TicketCode.Infrastructure.Data;

namespace TicketCode.Core.Services
{
    public abstract class CommonService
    {
        /// <summary>
        /// code缓存key
        /// </summary>
        protected readonly string KEY_TC_GROUP_LIST = ".{0}.vd";

        protected IRepository<TcGroups> groupRepository = null;

        protected ILogger logger = null;
        protected IRedisCache redisCache = null;

        protected static object _lock = new object();

        public CommonService(IRepository<TcGroups> groupRepository, IRedisCache redisCache,ILoggerFactory loggerFactory)
        {
            this.groupRepository = groupRepository;

            this.redisCache = redisCache;
            this.logger = loggerFactory.CreateLogger<CommonService>();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public async Task InitGroup()
        {
            var listGroup = await this.groupRepository.Query()
                .AsNoTracking()
                .Where(x => !x.bDelete && !x.bDisable)
                .Select(x => x.iPrefixCode)
                .ToListAsync();

            if (listGroup.Count <= 0) return;

            foreach (var item in listGroup)
            {
                CheckGroupCapacity(item);
            }
        }

        /// <summary>
        /// 检查分组容量
        /// </summary>
        /// <param name="iPrefixCode"></param>
        public void CheckGroupCapacity(int iPrefixCode)
        { 
            lock (_lock)
            {
                var ckGroup = this.groupRepository.Query()
                    .Where(x => x.iPrefixCode == iPrefixCode)
                    .Take(1)
                    .SingleOrDefault();

                if (ckGroup == null || ckGroup.bDelete || ckGroup.bDisable)
                    return;

                string key = string.Format(this.KEY_TC_GROUP_LIST, ckGroup.iPrefixCode);
                var len = this.redisCache.ListLength(key);
                if (len < ckGroup.iMinNumber)
                {
                    int max = (int)Math.Pow(10, ckGroup.iLength+1) - 1;

                    if (ckGroup.iUsedNumber >= max)
                        throw new OverflowException($"{ckGroup.iPrefixCode}分组code容量达到最大值");

                    long incr = ckGroup.iIncrNumber;
                    if (ckGroup.iIncrNumber + ckGroup.iUsedNumber > max)
                    {
                        incr = max - ckGroup.iUsedNumber;
                    }

                    //低于最低值，初始化
                    List<string> values = new List<string>();
                    for (int i = 1; i <= incr; i++)
                    {
                        values.Add(Convert.ToString(ckGroup.iUsedNumber + i));
                    }

                    //重新排序
                    values.Sort(delegate (string a, string b) { return (new Random()).Next(-1, 1); });
                    ckGroup.iUsedNumber += ckGroup.iIncrNumber;
                    this.redisCache.ListRightPush(key, values.ToArray<string>());
                    ckGroup.iCurrAvaNumber = (int)len + values.Count;
                }
                else
                {
                    ckGroup.iCurrAvaNumber = (int)len;
                }
                this.groupRepository.SaveChanges();
            }
        }

        /// <summary>
        /// 获取完整码
        /// </summary>
        /// <param name="iPrefixCode">前缀</param>
        /// <param name="iCode">取票码</param>
        /// <param name="iCodeLength">取票码长度</param>
        /// <returns></returns>
        protected long GetFullCode(int iPrefixCode, int iCode, int iCodeLength)
        {
            string code = iPrefixCode + iCode.ToString().PadLeft(iCodeLength, '0');
            return long.Parse(code + GetCheckValue(code));
        }

        /// <summary>
        /// 各数字位数字和取个位，如12345=》1+2+3+4+5 =》 5
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private long GetCheckValue(string code)
        {
            long value = 0;
            for (int i = 0; i < code.Length; i++)
            {
                value += long.Parse(code.Substring(i, 1));
            }
            return Convert.ToInt64(value.ToString().Last().ToString());
        }
    }
}
