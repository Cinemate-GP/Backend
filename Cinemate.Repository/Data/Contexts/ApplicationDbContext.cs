﻿using Cinemate.Core.Entities.Auth;
using Cinemate.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Claims;

namespace Cinemate.Repository.Data.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
		{
			_httpContextAccessor = httpContextAccessor;
		}
		public DbSet<Cast> Casts { get; set; }
		public DbSet<Genre> Genres { get; set; }
		public DbSet<Movie> Movies { get; set; }
		public DbSet<UserCastFollow> UserCastFollows { get; set; }
		public DbSet<UserFollow> UserFollows { get; set; }
		public DbSet<UserLikeMovie> UserLikeMovies { get; set; }
		public DbSet<UserRateMovie> UserRateMovies { get; set; }
		public DbSet<UserWatchedMovie> UserWatchedMovies { get; set; }
		public DbSet<UserReviewMovie> UserReviewMovies { get; set; }
		public DbSet<UserMovieWatchList> UserMovieWatchList { get; set; }
		public DbSet<CastMovie> CastMovie { get; set; }
		public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<Notification> Notifications { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			base.OnModelCreating(modelBuilder);
		}
		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			var entries = ChangeTracker.Entries<AuditableEntity>();
			foreach (var entityEntry in entries)
			{
				var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
				if (entityEntry.State == EntityState.Added)
				{
					entityEntry.Property(x => x.CreatedById).CurrentValue = currentUserId;
				}
				if (entityEntry.State == EntityState.Modified)
				{
					entityEntry.Property(x => x.UpdatedById).CurrentValue = currentUserId;
					entityEntry.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;
				}

			}
			return base.SaveChangesAsync(cancellationToken);
		}
	}
}
