using Cinemate.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Repository.Data.Configuration
{
	class CastMovieConfiguration : IEntityTypeConfiguration<CastMovie>
	{
		public void Configure(EntityTypeBuilder<CastMovie> builder)
		{
			builder.HasKey(x => new { x.CastId, x.TmdbId });
			builder.HasOne(x => x.Cast)
					.WithMany(x => x.CastMovies)
					.HasForeignKey(x => x.CastId)
					.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(x => x.Movie)
					.WithMany(x => x.CastMovies)
					.HasForeignKey(x => x.TmdbId)
					.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
