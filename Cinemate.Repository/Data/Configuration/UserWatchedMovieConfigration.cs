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
    class UserWatchedMovieConfigration : IEntityTypeConfiguration<UserWatchedMovie>
    {
        public void Configure(EntityTypeBuilder<UserWatchedMovie> builder)
        {
            builder.HasKey(uwm => new { uwm.UserId, uwm.TMDBId });            
            builder.HasOne(uwm => uwm.User)
                .WithMany(u => u.WatchedMovies)
                .HasForeignKey(uwm => uwm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(uwm => uwm.Movie)
                .WithMany(m => m.UserWatched)
                .HasForeignKey(uwm => uwm.TMDBId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(uwm => uwm.WatchedOn)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
