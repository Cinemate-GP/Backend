using Cinemate.Repository.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using System.Reflection;
using System.Threading.RateLimiting;

namespace Cinemate.API
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddAPIDependencies(this IServiceCollection services,
			IConfiguration configuration)
		{
			services.AddControllers();

			services.AddCors(options =>
				options.AddDefaultPolicy(builder =>
					builder
						.AllowAnyMethod()
						.AllowAnyHeader()
						.WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>()!)
				)
			);
			services.AddSwaggerServices();
			return services;
		}
		private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
		{
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

			return services;
		}
	}
}
