using Cinemate.Core.Service_Contract;
using Cinemate.Service.Services.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cinemate.Service
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddServicesDependencyInjection(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<IAuthService, AuthService>();
			return services;
		}
        private static IServiceCollection AddBackgroundJobsConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config => config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(configuration.GetConnectionS, string("HangfireConnection")));

            services.AddHangfireServer();

            return services;
        }




    }
}
