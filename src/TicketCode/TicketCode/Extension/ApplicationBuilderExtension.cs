﻿using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketCode.WebHost.Extension
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UseCustomedDistributedRedisCache(this IApplicationBuilder app)
        {
            return app;
        }
    }
}