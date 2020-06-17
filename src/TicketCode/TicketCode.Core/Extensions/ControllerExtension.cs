using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TicketCode.Core.Extensions
{
    public static class ControllerExtension
    {
        public static int GetAccountId(this ControllerBase controller)
        {
            var principal = System.Threading.Thread.CurrentPrincipal as System.Security.Claims.ClaimsPrincipal;
            if (principal == null) return 0;
            var claim = principal.Claims.Where(x => x.Type == "accountid").FirstOrDefault();
            if (claim != null)
                return int.Parse(claim.Value);
            return 0;
        }

        public static int GetGroup(this ControllerBase controller)
        {
            var principal = System.Threading.Thread.CurrentPrincipal as System.Security.Claims.ClaimsPrincipal;
            if (principal == null) return 0;
            var claim = principal.Claims.Where(x => x.Type == "group").FirstOrDefault();
            if (claim != null)
                return int.Parse(claim.Value);
            return 0;
        }

        public static string GetReqNo(this ControllerBase controller)
        {
            return controller.Request.Query["reqno"].FirstOrDefault();
        }


    }
}
