using Cinemate.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Repository.Data.Configuration
{
    public class CastConfiguration : IEntityTypeConfiguration<Cast>
	{
		public void Configure(EntityTypeBuilder<Cast> builder)
		{
			builder.HasKey(c => c.Id);
			builder.Property(c => c.Id).ValueGeneratedNever();
		}
	}
}
