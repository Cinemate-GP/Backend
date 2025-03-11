using Cinemate.Core.Abstractions.Consts;
using Cinemate.Core.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinemate.Repository.Data.Configuration.Auth
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

			builder.Property(x => x.FullName).HasMaxLength(100);

			//Default Data
			builder.HasData(new ApplicationUser
			{
				Id = DefaultUsers.AdminId,
				FullName = DefaultUsers.AdminFullName,
				Gender = DefaultUsers.AdminGender,
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
