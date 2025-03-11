using Cinemate.Core.Abstractions.Consts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Repository.Data.Configuration.Auth
{
	public class UserRoleConfigration : IEntityTypeConfiguration<IdentityUserRole<string>>
	{
		public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
		{
			//Default Data
			builder.HasData(new IdentityUserRole<string>
			{
				UserId = DefaultUsers.AdminId,
				RoleId = DefaultRoles.AdminRoleId
			});
		}
	}
}
