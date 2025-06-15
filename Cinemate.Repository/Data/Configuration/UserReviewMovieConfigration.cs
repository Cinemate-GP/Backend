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
    class UserReviewMovieConfigration : IEntityTypeConfiguration<UserReviewMovie>
    {
        public void Configure(EntityTypeBuilder<UserReviewMovie> builder)
        {
            // i have 3 primary keys in my table two of them are foreign keys and one is a normal key
            builder.HasKey(urm => new { urm.UserId, urm.TMDBId, urm.ReviewId });

            // make identity column for review id
            builder.Property(urm => urm.ReviewId)
                .ValueGeneratedOnAdd();     
            
            builder.HasOne(urm => urm.User)
                .WithMany(u => u.ReviewedMovies)
                .HasForeignKey(urm => urm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(urm => urm.Movie)
                .WithMany(m => m.UserReviews)
                .HasForeignKey(urm => urm.TMDBId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(urm => urm.ReviewBody).HasMaxLength(512);

            builder.Property(urm => urm.ReviewConfidence)
                .HasPrecision(8, 2);

			builder.Property(urm => urm.ReviewedOn)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
