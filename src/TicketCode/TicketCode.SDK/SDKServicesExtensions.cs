using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;


namespace HxSDK.TicketCode
{
    public static class SDKServicesExtensions
    {
        /// <summary>
        /// Adds sdk <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">An <see cref="Action{SdkOption}"/> to configure the provided <see cref="SdkOption"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddHxTicketCode(this IServiceCollection services, Action<SdkOption> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            services.AddOptions();

            services.AddTransient<IHxTicketCode, HxTicketCode>();

            services.Configure(setupAction);

            return services;
        }
    }
}
