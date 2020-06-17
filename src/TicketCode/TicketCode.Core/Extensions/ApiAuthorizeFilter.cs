using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using TicketCode.Core.Models;
using TicketCode.Infrastructure;
using TicketCode.Infrastructure.Data;
using TicketCode.Infrastructure.Extensions;
using System.Security.Principal;
using System.Security.Claims;
using System.Collections;
using System.Collections.Generic;

namespace TicketCode.Core.Extensions
{
    public class ApiAuthorizeFilter : Microsoft.AspNetCore.Mvc.Filters.IAuthorizationFilter
    {
        private IRepository<TcAccounts> accountRepository = null;

        public ApiAuthorizeFilter(IRepository<TcAccounts> accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var request = context.HttpContext.Request;

            string reqno = request.Query["reqno"];
            if (string.IsNullOrWhiteSpace(reqno))
            {
                context.Result = ErrorHandle(Result.Fail("reqno有误",""));
                return;
            }

            string appid = request.Query["appid"];
            if (string.IsNullOrWhiteSpace(appid))
            {
                context.Result = ErrorHandle(Result.Fail("appid不能为空", reqno));
                return;
            }

            var account = this.accountRepository.Query().AsNoTracking()
                .Where(x => x.sAppId == appid && !x.bDisable)
                .SingleOrDefault();

            if (account == null)
            {
                context.Result = ErrorHandle(Result.Fail("appid无效", reqno));
                return;
            }

            string timestamp = request.Query["timestamp"];
            if (!IsValidTimestamp(timestamp))
            {
                context.Result = ErrorHandle(Result.Fail("timestamp无效", reqno));
                return;
            }

            string group = request.Query["group"];
            if (string.IsNullOrWhiteSpace(group) || !Regex.IsMatch(group, "^\\d+$"))
            {
                context.Result = ErrorHandle(Result.Fail("group有误", reqno));
                return;
            }

            string sign = request.Query["sign"];
            if (string.IsNullOrWhiteSpace(sign) || !GetSign(appid, group, reqno, timestamp, account.sAppSecret).Equals(sign))
            {
                context.Result = ErrorHandle(Result.Fail("sign有误", reqno));
                return;
            }

            IList<Claim> clamins = new List<Claim>();
            clamins.Add(new Claim("appid", account.sAppId));
            clamins.Add(new Claim("accountid", account.id.ToString()));
            clamins.Add(new Claim("group", group));

            System.Threading.Thread.CurrentPrincipal = new ClaimsPrincipal(new ClaimsIdentity(clamins));
        }

        private bool IsValidTimestamp(string timestamp)
        {
            long tick;
            if (!long.TryParse(timestamp, out tick))
                return false;
            return new DateTime(1970, 1, 1).Add(new TimeSpan(tick * 10000000)) < DateTime.UtcNow.AddMinutes(2);
        }

        private JsonResult ErrorHandle(Result ret)
        {
            JsonResult json = new JsonResult(ret);
            return json;
        }

        private string GetSign(string appid, string group, string reqno, string timestamp, string key)
        {
            //appid+group+reqno+timestamp+key
            return $"{appid}{group}{reqno}{timestamp}{key}".ToMd5().ToLower();
        }
    }
}
