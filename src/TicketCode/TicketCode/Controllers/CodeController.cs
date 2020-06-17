using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Markup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Org.BouncyCastle.Ocsp;
using TicketCode.Core.Dto;
using TicketCode.Core.Redis;
using TicketCode.Core.Services;
using TicketCode.Core.Extensions;
using System.IO;
using Newtonsoft.Json;
using TicketCode.Infrastructure;

namespace TicketCode.WebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeController : ControllerBase
    {
        private IRequestService requestService = null;

        public CodeController(IRequestService requestService)
        {
            this.requestService = requestService;
        }

        /// <summary>
        /// 获取取票码
        /// </summary>
        /// <returns></returns>
        [HttpPost("get")]
        public async Task<Result<DtoGetCodeResponse>> Get([FromBody] DtoGetCodeRequest request)
        {
            var result = await this.requestService.GetCode(this.GetAccountId(), this.GetGroup(), request);
            result.reqno = this.GetReqNo();
            return result;
        }

        /// <summary>
        /// 核销码反查订单
        /// </summary>
        /// <returns></returns>
        [HttpPost("query")]
        public async Task<IActionResult> Query(string sCode = "")
        {
            return null;
        }

        /// <summary>
        /// 核销取票码
        /// </summary>
        /// <returns></returns>
        [HttpPost("consume")]
        public async Task<Result> Post(string sNo = "", int? iCode = null)
        {
            if (string.IsNullOrWhiteSpace(sNo))
                return Result.Fail("sNo不能为空", this.GetReqNo());

            if (!iCode.HasValue || iCode.Value <= 0)
                return Result.Fail("iCode无效", this.GetReqNo());

            var result = await this.requestService.ConsumeCode(this.GetAccountId(), sNo, iCode.Value);
            result.reqno = this.GetReqNo();
            return result;
        }


    }
}