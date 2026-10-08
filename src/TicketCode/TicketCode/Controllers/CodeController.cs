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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace TicketCode.WebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeController : ControllerBase
    {
        ILogger logger;

        private IRequestService requestService = null;

        public CodeController(IRequestService requestService,ILogger<CodeController> logger)
        {
            this.logger = logger;
            this.requestService = requestService;
        }

        /// <summary>
        /// 获取取票码
        /// </summary>
        /// <returns></returns>
        [HttpPost("get")]
        public async Task<Result<DtoGetCodeResponse>> Get([FromBody] DtoGetCodeRequest request)
        {
            this.logger.LogInformation("取码请求：{@request}",request);

            DateTime dt;
            if (!DateTime.TryParse(request.tExpireTime, out dt))
                return Result.Fail<DtoGetCodeResponse>("失效时间格式错误", this.GetReqNo());

            var result = await this.requestService.GetCode(this.GetAccountId(), this.GetGroup(), request);
            result.reqno = this.GetReqNo();

            this.logger.LogInformation("取码返回:{@response}", result);

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
        public async Task<Result> Post([FromBody]DtoConsumeCodeRequest request)
        {
            //if (string.IsNullOrWhiteSpace(sNo))
            //    return Result.Fail("sNo不能为空", this.GetReqNo());

            //if (!iCode.HasValue || iCode.Value <= 0)
            //    return Result.Fail("iCode无效", this.GetReqNo());

            this.logger.LogInformation("核码请求：{@request}", request);

            var result = await this.requestService.ConsumeCode(this.GetAccountId(), request.sNo, request.aCodes);
            result.reqno = this.GetReqNo();

            this.logger.LogInformation("核码返回：{@request}", result);

            return result;
        }

        //[HttpGet("temprecycle")]
        //public async Task TemporaryRecycle()
        //{
        //    //await this.requestService.TemporaryRecycle();
        //}

        [Route("/error")]
        [AllowAnonymous]
        public Result SystemError()
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var error = feature?.Error;
            if (error != null)
            {
                if (error.InnerException != null)
                    logger.LogError(error.InnerException.Message, error.InnerException);
                else
                    logger.LogError(error.Message, error);
            }
            return Result.Fail("服务器异常", "");
        }

        [Route("/demo")]
        [AllowAnonymous]
        public async Task<Result> Demo()
        {
            var result = await this.requestService.GetCode(1, 11, new DtoGetCodeRequest()
            {
                iNumber = 5,
                sOuterNo = Guid.NewGuid().ToString(),
                tExpireTime = DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss")
            });
            return result;
        }
    }
}