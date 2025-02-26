using Cinemate.Core.Abstractions.Consts;
using Cinemate.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Repository.Data.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
	{
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
		{
			//Default Data
			builder.HasData(
			new ApplicationRole
			{
				Id = DefaultRoles.AdminRoleId,
				Name = DefaultRoles.Admin,
				NormalizedName = DefaultRoles.Admin.ToUpper(),
				ConcurrencyStamp = DefaultRoles.AdminRoleConcurrencyStamp
			},
			new ApplicationRole
			{
				Id = DefaultRoles.MemberRoleId,
				Name = DefaultRoles.Member,
				NormalizedName = DefaultRoles.Member.ToUpper(),
				ConcurrencyStamp = DefaultRoles.MemberRoleConcurrencyStamp,
				IsDefault = true
			});
		}
	}
}
