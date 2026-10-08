using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCode.Core.Dto;
using TicketCode.Core.Models;
using TicketCode.Core.Redis;
using TicketCode.Infrastructure;
using TicketCode.Infrastructure.Data;

namespace TicketCode.Core.Services
{
    public class RequestService : CommonService, IRequestService
    {
        private IRepository<TcRequsets> requestRepository = null;
        private IRepository<TcConsume> consumeRepository = null;
        private IRepository<TcRequestLines> lineRepository = null;

        public RequestService(/*IRedisCache redisCache,*/ ILoggerFactory loggerFactory,
            IRepository<TcGroups> groupRepository,
            IRepository<TcRequsets> requestRepository,
            IRepository<TcRequestLines> lineRepository,
            IRepository<TcConsume> consumeRepository) : base(groupRepository, loggerFactory)
        {
            this.requestRepository = requestRepository;
            this.lineRepository = lineRepository;
            this.consumeRepository = consumeRepository;
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="iAccountId"></param>
        /// <param name="iPrefixCode"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Result<DtoGetCodeResponse>> GetCode(int iAccountId, int iPrefixCode, DtoGetCodeRequest request)
        {
            if (await this.requestRepository.Query()
                .Include(x => x.TcGroup)
                .Where(x => x.iAccountId == iAccountId && x.TcGroup.iPrefixCode == iPrefixCode && x.sOuterNo == request.sOuterNo)
                .AnyAsync())
                return Result.Fail<DtoGetCodeResponse>($"{request.sOuterNo}已存在", "");

            var group = await this.groupRepository.Query().AsNoTracking()
                .Where(x => x.iPrefixCode == iPrefixCode)
                .SingleOrDefaultAsync();

            if (group == null || group.bDelete || group.bDisable)
                return Result.Fail<DtoGetCodeResponse>($"分组{iPrefixCode}已无效", "");

            var tc = new TcRequsets()
            {
                iAccountId = iAccountId,
                iGroupId = group.id,
                iNumber = request.iNumber,
                sMemo = request.sMemo,
                sOuterNo = request.sOuterNo,
                sRequestNo = SequenceService.GetId(iPrefixCode),
                tCreateTime = DateTime.Now,
                tExpireTime = DateTime.Parse(request.tExpireTime),
            };

            CheckGroupCapacity(iPrefixCode);

            var infoGroup = await this.groupRepository.Query().AsNoTracking()
                .Where(x => x.iPrefixCode == iPrefixCode)
                .SingleOrDefaultAsync();

            var listline = new List<TcRequestLines>();
            for (int i = 1; i <= request.iNumber; i++)
            {
                string code = await RedisHelper.LPopAsync(string.Format(this.KEY_TC_GROUP_LIST, iPrefixCode));
                //string code = await this.redisCache.ListLeftPopAsync(string.Format(this.KEY_TC_GROUP_LIST, iPrefixCode));
                if (string.IsNullOrWhiteSpace(code))
                    return Result.Fail<DtoGetCodeResponse>("取码失败，没有足够的凭证码", "");
                //throw new Exception("");
                TcRequestLines line = new TcRequestLines()
                {
                    iCode = int.Parse(code),
                    iPrefixCode = iPrefixCode,
                    iFullCode = GetFullCode(iPrefixCode, int.Parse(code), infoGroup.iLength),
                    tExpireTime = tc.tExpireTime
                };
                listline.Add(line);
            }
            tc.TcRequestLines = listline;

            this.requestRepository.Add(tc);
            await this.requestRepository.SaveChangesAsync();

            return Result.Ok<DtoGetCodeResponse>(new DtoGetCodeResponse()
            {
                iLength = infoGroup.iLength,
                tExpireTime = request.tExpireTime,
                iPrefixCode = iPrefixCode,
                sOuterNo = request.sOuterNo,
                sTcNo = tc.sRequestNo,
                aNumbers = tc.TcRequestLines.Select(x => x.iFullCode)
            }, "");
        }

        /// <summary>
        /// 核销取票码
        /// </summary>
        /// <param name="iAccountId"></param>
        /// <param name="iPrefixCode"></param>
        /// <param name="sOuterNoOrTcNo"></param>
        /// <param name="iFullCode"></param>
        /// <returns></returns>
        public async Task<Result> ConsumeCode(int iAccountId, string sOuterNoOrTcNo, long[] sFullCodes)
        {
            //using (var tran = this.lineRepository.BeginTransaction())
            {
                try
                {
                    long iGroupId = 0;
                    int iConsumeQty = sFullCodes.Length;
                    IList<string> sConsumeCodes = new List<string>();
                    foreach (var iFullCode in sFullCodes)
                    {
                        var line = await this.lineRepository.Query()
                            .Include(x => x.TcRequset)
                            .ThenInclude(x => x.TcGroup)
                            .Where(x => x.iFullCode == iFullCode)
                            .Where(x => x.TcRequset.sOuterNo == sOuterNoOrTcNo || x.TcRequset.sRequestNo == sOuterNoOrTcNo)
                            .SingleOrDefaultAsync();
                        if (line == null) throw new ExceptionResult($"取票码{iFullCode}无效");
                        //return Result.Fail($"取票码{iFullCode}无效", "");

                        iGroupId = line.TcRequset.TcGroup.iPrefixCode;

                        if (line.bConsume) continue;
                        //return Result.Fail($"已于{line.tConsumeTime.Value.ToString("yyyy/MM/dd HH:mm:ss")}核销", "");

                        line.bConsume = true;
                        line.tConsumeTime = DateTime.Now;

                        TcConsume consume = new TcConsume()
                        {
                            iAccountId = iAccountId,
                            iGroupId = line.TcRequset.iGroupId,
                            iFullCode = iFullCode,
                            iRequestLineId = line.id,
                            tConsumeTime = DateTime.Now
                        };
                        this.consumeRepository.Add(consume);
                        sConsumeCodes.Add(line.iCode.ToString());
                    }

                    await this.lineRepository.SaveChangesAsync();
                    await this.consumeRepository.SaveChangesAsync();

                    //tran.Commit();

                    //将已核销的凭证码放入码池
                    if (sConsumeCodes.Count > 0)
                    {
                        await RedisHelper.RPushAsync(string.Format(this.KEY_TC_GROUP_LIST, iGroupId), sConsumeCodes.ToArray<string>());
                        //await this.redisCache.ListRightPushAsync(string.Format(this.KEY_TC_GROUP_LIST, iGroupId), sConsumeCodes.ToArray<string>());
                    }
                }
                catch (ExceptionResult ex)
                {
                    //tran.Rollback();
                    return Result.Fail(ex.Message, "");
                }
                catch (Exception ex)
                {
                    //tran.Rollback();
                    this.logger.LogError(ex, "核销异常：{message}，凭证码：{@codes}", ex.Message, sFullCodes);
                    return Result.Fail("核销异常", "");
                }
            }

            return Result.Ok("核销成功");
        }

        /// <summary>
        /// code 过期回收
        /// </summary>
        /// <returns></returns>
        public async Task RecycleCode()
        {
            int count = 0;
            try
            {
                this.logger.LogInformation("开始执行回收");
                var query = await this.lineRepository.Query()
                //.Include(x => x.TcRequset)
                //.ThenInclude(x => x.TcGroup)
                .Where(x => !x.bConsume && x.tExpireTime < DateTime.Now)
                //2022.5.10 通过冗余字段查询
                //.Where(x => !x.bConsume && (x.TcRequset.tExpireTime < DateTime.Now || x.TcRequset.tCreateTime > x.TcRequset.tExpireTime))
                .OrderBy(x => x.id)
                .Take(10)
                .ToListAsync();

                if (query == null || query.Count <= 0)
                {
                    this.logger.LogInformation("本次暂无需要回收的码");
                    return;
                }
                
                foreach (var line in query)
                {
                    line.bConsume = true;
                    line.tConsumeTime = DateTime.Now;
                    line.iConsumeType = 1;
                    await this.lineRepository.SaveChangesAsync();
                    //await this.redisCache.ListRightPushAsync(string.Format(this.KEY_TC_GROUP_LIST, line.iPrefixCode), new string[] { line.iCode.ToString() });
                    await RedisHelper.RPushAsync(string.Format(this.KEY_TC_GROUP_LIST, line.iPrefixCode), new string[] { line.iCode.ToString() });
                    System.Threading.Thread.Sleep(10);
                    count++;
                }
                this.logger.LogWarning("本次回收{count}个码", count);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "码回收异常:{message}，本次已回收码：{count}", ex.Message, count);
            }
        }

        public async Task TemporaryRecycle()
        {
            //核销表最后一条异常记录（13821	49469	2100002915	2020-09-28 09:50:58.059248	4	4）
            // tcrequsets(	41424	102009281015420133	HXZPT11601288139234	1	2020-09-28 10:15:42.472008	2020-09-28 23:59:59.000000	2	3	)
            //回收9月20日之前的
            var list = await this.lineRepository.Query()
                .Include(x => x.TcRequset)
                .ThenInclude(x => x.TcGroup)
                .Where(x => !x.bConsume && (x.TcRequset.tCreateTime < x.TcRequset.tExpireTime || x.TcRequset.tExpireTime < DateTime.Now))
                .OrderBy(x => x.id)
                .Take(100)
                .ToListAsync();
            if (list.Count <= 0)
                return;
            int count = 0;
            foreach (var line in list)
            {
                if (!line.bConsume)
                {
                    line.bConsume = true;
                    line.tConsumeTime = DateTime.Now;
                    line.iConsumeType = 1;
                    //await this.lineRepository.SaveChangesAsync();
                }
                //await this.redisCache.ListRightPushAsync(string.Format(this.KEY_TC_GROUP_LIST, line.TcRequset.TcGroup.iPrefixCode), new string[] { line.iCode.ToString() });
                System.Threading.Thread.Sleep(10);
                count++;
            }
            await TemporaryRecycle();
        }
    }
}
