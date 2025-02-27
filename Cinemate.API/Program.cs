using Cinemate.Core;
using Cinemate.Repository;
using Cinemate.Service;

namespace Cinemate.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddAPIDependencies(builder.Configuration);
            builder.Services.AddRepositoryDependencyInjection(builder.Configuration);
            builder.Services.AddCoreDependencyInjection(builder.Configuration);
            builder.Services.AddServicesDependencyInjection(builder.Configuration);

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
