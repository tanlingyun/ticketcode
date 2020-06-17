using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketCode.Core.Data;
using TicketCode.Core.Services;
using TicketCode.Infrastructure.Data;

namespace TicketCode.WebHost.Extension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCustomedCodeStore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(option =>
            {
                option.InstanceName = "tkc";
                option.Configuration = configuration.GetConnectionString("RedisConnection");

            });
            return services;
        }

        public static IServiceCollection AddCustomizedDataStore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<TcDbContext>(options =>
                options.UseMySql(configuration.GetConnectionString("MysqlConnection"),
                    b => b.MigrationsAssembly("TicketCode.WebHost")));
            return services;
        }

        public static IServiceCollection AddCustomizedServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient(typeof(IRepositoryWithTypedId<,>), typeof(RepositoryWithTypedId<,>));

            services.AddScoped<IRequestService, RequestService>();

            return services;
        }

        public static IServiceCollection ConfigureCustomizedApiBehavior(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    //获取验证失败的模型字段 
                    var errors = context.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => e.Value.Errors.First().ErrorMessage)
                    .ToList();
                    var str = string.Join("|", errors);
                    var result = new
                    {
                        code = -1,
                        reqno = context.HttpContext.Request.Query["reqno"].FirstOrDefault(),
                        message = str
                    };
                    return new BadRequestObjectResult(result);
                };
            });
            return services;
        }
    }
}
