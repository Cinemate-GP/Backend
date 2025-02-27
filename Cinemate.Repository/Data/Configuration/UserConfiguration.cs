using Cinemate.Core.Abstractions.Consts;
using Cinemate.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinemate.Repository.Data.Configuration
{
	public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder
				.OwnsMany(x => x.RefreshTokens)
				.ToTable("RefreshTokens")
				.WithOwner()
				.HasForeignKey("UserId");

			builder.Property(x => x.FirstName).HasMaxLength(100);
			builder.Property(x => x.LastName).HasMaxLength(100);

			//Default Data
			builder.HasData(new ApplicationUser
			{
				Id = DefaultUsers.AdminId,
				FirstName = "Cinemate System",
				LastName = "Admin",
				UserName = DefaultUsers.AdminEmail,
				BirthDay = new DateOnly(2000, 1, 1),
				NormalizedUserName = DefaultUsers.AdminEmail.ToUpper(),
				Email = DefaultUsers.AdminEmail,
				NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
				SecurityStamp = DefaultUsers.AdminSecurityStamp,
				ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
				EmailConfirmed = true,
				PasswordHash = "AQAAAAIAAYagAAAAEAR2V+bcDJAlzUiuTRqKkLj/Uv4ibKCWikvvMF1g75/iOokLhV1l9SedoJOqspT0mA=="
			});
		}
	}
}
