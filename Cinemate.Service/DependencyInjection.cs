using Cinemate.Core.Entities;
using Cinemate.Core.Helpers;
using Cinemate.Core.Service_Contract;
using Cinemate.Service.Services.Actors;
using Cinemate.Service.Services.Authentication;
using Cinemate.Service.Services.File;
using Cinemate.Service.Services.Follow_Service;
using Cinemate.Service.Services.Movies;
using Cinemate.Service.Services.Profile;
using Cinemate.Service.Services.User_Like_Movie;
using Cinemate.Service.Services.User_Rate_Movie;
using Cinemate.Service.Services.User_Review_Movie;
using Cinemate.Service.Services.User_Watched_Movie;
using Cinemate.Service.Services.User_Watchlist_Movie;
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
			services.AddScoped<IMovieService, MovieService>();
			services.AddScoped<IActorService, ActorService>();
			services.AddScoped<IProfileService, ProfileService>();
			services.AddScoped<IFileService, FileService>();
			services.AddScoped<IUserLikeMovieService, UserLikeMovieService>();
			services.AddScoped<IUserWatchedMovieService, UserWatchedMovieService>();
			services.AddScoped<IUserRateMovieService, UserRateMovieService>();
			services.AddScoped<IUserReviewMovieService, UserReviewMovieService>();
			services.AddScoped<IUserWatchlistMovieService, UserWatchlistMovieService>();
			services.AddScoped<IUserfollowService, UserFollowService>();
            services.AddScoped<INotificationService, NotificationService>();
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
