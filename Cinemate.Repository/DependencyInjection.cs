using Cinemate.Core.Authentication_Contract;
using Cinemate.Core.Entities.Auth;
using Cinemate.Repository.Authentication;
using Cinemate.Repository.Data.Contexts;
using Cinemate.Repository.Errors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Cinemate.Repository
{
    public static class DependencyInjection
	{
		public static IServiceCollection AddRepositoryDependencyInjection(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddExceptionHandler<GlobalExceptionHandler>();
			services.AddProblemDetails();

			var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection String DefaultConnection not found.");
			services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
			services.AddAuthConfig(configuration);
			return services;
		}
		private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddIdentity<ApplicationUser, ApplicationRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			services.AddSingleton<IJwtProvider, JwtProvider>();
			services.AddOptions<JwtOptions>().BindConfiguration(JwtOptions.SectionName).ValidateDataAnnotations().ValidateOnStart();

			var jwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(o =>
				{
					o.SaveToken = true;
					o.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key!)),
						ValidIssuer = jwtSettings?.Issuer,
						ValidAudience = jwtSettings?.Audience
					};
				});
			services.Configure<IdentityOptions>(options =>
			{
				options.Password.RequiredLength = 8;
				options.SignIn.RequireConfirmedEmail = true;
				options.User.RequireUniqueEmail = true;
				options.User.AllowedUserNameCharacters =
					   "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
			});

			return services;
		}
	}
}
