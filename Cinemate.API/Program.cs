using Cinemate.Core;
using Cinemate.Core.Service_Contract;
using Cinemate.Repository;
using Cinemate.Repository.Data.Contexts;
using Cinemate.Service;
using Hangfire;
using HangfireBasicAuthenticationFilter;

namespace Cinemate.API
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddAPIDependencies(builder.Configuration);
			builder.Services.AddRepositoryDependencyInjection(builder.Configuration);
			builder.Services.AddCoreDependencyInjection(builder.Configuration);
			builder.Services.AddServicesDependencyInjection(builder.Configuration);
			builder.Services.AddSignalR(options =>
			{
				options.EnableDetailedErrors = true;
			});

			var app = builder.Build();

			using var scope = app.Services.CreateAsyncScope();
			var services = scope.ServiceProvider;
			var context = services.GetRequiredService<ApplicationDbContext>();
			// await ApplicationContextSeed.SeedAsync(context);
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCors();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseHangfireDashboard("/jobs", new DashboardOptions
			{
				Authorization =
				[
					new HangfireCustomBasicAuthenticationFilter{
						User=app.Configuration.GetValue<string>("HangfireSettings:Username"),
						Pass=app.Configuration.GetValue<string>("HangfireSettings:Password")
					}
				],
				DashboardTitle = "Cinemate Dashboard",
			});
			RecurringJob.AddOrUpdate<IMovieService>("daily-upcoming-movie-check",service => service.UpCommingMovieAsync(CancellationToken.None),"1 0 * * *");
			RecurringJob.AddOrUpdate<IMovieService>("daily-update-movie-rating",service => service.UpdateMovieRatingsAsync(CancellationToken.None),"1 0 * * *");

			app.MapControllers();
			app.MapHub<NotificationHub>("/notificationHub").RequireCors("SignalRCors");

			app.Run();
		}
	}
}
