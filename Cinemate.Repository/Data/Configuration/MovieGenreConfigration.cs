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
	public class MovieGenreConfigration : IEntityTypeConfiguration<MovieGenre>
	{
		public void Configure(EntityTypeBuilder<MovieGenre> builder)
		{
			builder.HasKey(x => new { x.TMDBId, x.GenreId });
			builder.HasOne(x => x.Movie)
					.WithMany(x => x.MovieGenres)
					.HasForeignKey(x => x.TMDBId)
					.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(x => x.Genre)
					.WithMany(x => x.MovieGenres)
					.HasForeignKey(x => x.GenreId)
					.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
