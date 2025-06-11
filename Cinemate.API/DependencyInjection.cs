namespace Cinemate.API
{
	public static class DependencyInjection
	{		
		public static IServiceCollection AddAPIDependencies(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddControllers();			
			services.AddCors(options =>
			{
				options.AddDefaultPolicy(builder =>
					builder
						.WithOrigins(
							"http://localhost:3000",
							"https://localhost:7098", 
							"http://localhost:7098", 
							"https://localhost:3000",
							"http://cinemate.runasp.net",
							"https://cinemate.runasp.net",
                            "https://cinemate-eosin.vercel.app",
							"https://cinemate-eosin.vercel.app",
							"http://cinemategp.netlify.app",
							"http://cinemategp.netlify.app"
						)
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials()
				);
				
				// Additional policy for SignalR specifically
				options.AddPolicy("SignalRCors", builder =>
					builder
						.WithOrigins(
							"http://localhost:3000",
							"https://localhost:7098",
							"http://localhost:7098",
							"https://localhost:3000",
							"http://cinemate.runasp.net",
							"https://cinemate.runasp.net",
							"https://cinemate-eosin.vercel.app",
							"https://cinemate-eosin.vercel.app",
							"http://cinemategp.netlify.app",
							"http://cinemategp.netlify.app"
						)
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials()
				);
			});
			
			services.AddSwaggerServices();
            services.AddHttpContextAccessor();
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
