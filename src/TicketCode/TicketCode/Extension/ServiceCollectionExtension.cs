using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketCode.WebHost.Extension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCustomedDistributedRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDistributedRedisCache(option =>
            {
                option.InstanceName = "tkc";
                option.Configuration = configuration.GetConnectionString("RedisConnection");
            });
            return services;
        }
    }
}
