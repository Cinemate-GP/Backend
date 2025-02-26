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
