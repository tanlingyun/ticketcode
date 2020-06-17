using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TicketCode.Core.Extensions;
using TicketCode.Core.Services;
using TicketCode.WebHost.Extension;

namespace TicketCode.WebHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomizedServices();

            services.AddCustomedCodeStore(Configuration);

            services.AddCustomizedDataStore(this.Configuration);

            services.AddControllers(option =>
            {
                option.Filters.Add<ApiAuthorizeFilter>();
            });

            services.ConfigureCustomizedApiBehavior();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseWhen(
                    context => !context.Request.Path.StartsWithSegments("/api"),
                    a => a.UseExceptionHandler("/Home/Error")
                );
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
