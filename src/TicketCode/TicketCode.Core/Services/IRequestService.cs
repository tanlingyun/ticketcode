using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TicketCode.Core.Dto;
using TicketCode.Infrastructure;

namespace TicketCode.Core.Services
{
    public interface IRequestService
    {
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="iAccountId"></param>
        /// <param name="iPrefixCode"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<Result<DtoGetCodeResponse>> GetCode(int iAccountId, int iPrefixCode, DtoGetCodeRequest request);

        /// <summary>
        /// 核销取票码
        /// </summary>
        /// <param name="iAccountId"></param>
        /// <param name="iPrefixCode"></param>
        /// <param name="sOuterNoOrTcNo"></param>
        /// <param name="iFullCode"></param>
        /// <returns></returns>
        Task<Result> ConsumeCode(int iAccountId, string sOuterNoOrTcNo, int iFullCode);
    }
}
