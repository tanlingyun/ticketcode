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
    public class RequestService:CommonService,IRequestService
    {
        private IRepository<TcRequsets> requestRepository = null;

        private IRepository<TcConsume> consumeRepository = null;
        private IRepository<TcRequestLines> lineRepository = null;

        public RequestService(IRedisCache redisCache, ILoggerFactory loggerFactory,
            IRepository<TcGroups> groupRepository,
            IRepository<TcRequsets> requestRepository,
            IRepository<TcRequestLines> lineRepository,
            IRepository<TcConsume> consumeRepository) : base(groupRepository, redisCache, loggerFactory)
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

            var tc = new TcRequsets()
            {
                iAccountId = iAccountId,
                iGroupId = iPrefixCode,
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
                string code = await this.redisCache.ListLeftPopAsync(string.Format(this.KEY_TC_GROUP_LIST, iPrefixCode));
                if (string.IsNullOrWhiteSpace(code))
                    throw new Exception("");

                TcRequestLines line = new TcRequestLines()
                {
                    iCode = int.Parse(code),
                    iFullCode = GetFullCode(iPrefixCode, int.Parse(code), infoGroup.iLength)
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
            using (var tran = this.lineRepository.BeginTransaction())
            {
                try
                {
                    foreach (var iFullCode in sFullCodes)
                    {
                        var line = await this.lineRepository.Query()
                            .Include(x => x.TcRequset)
                            .Where(x => x.iFullCode == iFullCode)
                            .Where(x => x.TcRequset.sOuterNo == sOuterNoOrTcNo || x.TcRequset.sRequestNo == sOuterNoOrTcNo)
                            .SingleOrDefaultAsync();

                        if (line == null) throw new ExceptionResult($"取票码{iFullCode}无效");
                        //return Result.Fail($"取票码{iFullCode}无效", "");

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
                    }

                    await this.lineRepository.SaveChangesAsync();
                    await this.consumeRepository.SaveChangesAsync();
                    
                    tran.Commit();
                }
                catch (ExceptionResult ex)
                {
                    tran.Rollback();
                    return Result.Fail(ex.Message, "");
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }

            return Result.Ok("核销成功");
        }
    }
}
