using Cinemate.Core.Helpers;
using Cinemate.Core.Service_Contract;
using Cinemate.Service.Services.Authentication;
using Hangfire;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cinemate.Service
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddServicesDependencyInjection(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IEmailSender, EmailService>();
            services.AddBackgroundJobsConfig(configuration);
            return services;
		}
        private static IServiceCollection AddBackgroundJobsConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config => config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

            services.AddHangfireServer();

            return services;
        }




    }
}
