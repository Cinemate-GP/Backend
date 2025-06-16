using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mapster;
using MapsterMapper;
using System.Reflection;
using FluentValidation.AspNetCore;
using FluentValidation;
using Cinemate.Core.Helpers;
using Cinemate.Core.Services;

namespace Cinemate.Core
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddCoreDependencyInjection(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddMapsterConfig();
			services.AddFluentValidationConfig();
			services.AddSecurityServices();
			services.Configure<MailSetting>(configuration.GetSection(MailSetting.SectionName));
			return services;
		}
		private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
		{
			var mappingConfig = TypeAdapterConfig.GlobalSettings;
			mappingConfig.Scan(Assembly.GetExecutingAssembly());

			services.AddSingleton<IMapper>(new Mapper(mappingConfig));
           
            return services;
        }
		private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
		{
			services
				.AddFluentValidationAutoValidation()
				.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

			return services;
		}

		private static IServiceCollection AddSecurityServices(this IServiceCollection services)
		{
			services.AddScoped<IHtmlEncodingService, HtmlEncodingService>();

			return services;
		}
	}
}
