using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TicketCode.Core.Services;

namespace TicketCode.Core.HostService
{
    /// <summary>
    /// 后台任务
    /// </summary>
    public class BackgroundHostService : BackgroundService
    {
        protected readonly ILogger logger;
        protected IServiceProvider provider;

        public BackgroundHostService(IServiceProvider provider, ILogger<BackgroundHostService> logger)
        {
            this.logger = logger;
            this.provider = provider;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogWarning("后台服务已启动");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogWarning("后台服务已停止");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            logger.LogWarning("后台队列开始工作");
            while (!cancellationToken.IsCancellationRequested)
            {
                var scope = this.provider.CreateScope();
                var requestService = scope.ServiceProvider.GetService<IRequestService>();
                await requestService.RecycleCode();
                System.Threading.Thread.Sleep(1000);
            }
            logger.LogWarning("后台队列停止工作");
        }
    }
}
