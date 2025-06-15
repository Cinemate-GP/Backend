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
    class UserMovieWatchListConfigration : IEntityTypeConfiguration<UserMovieWatchList>
    {
        public void Configure(EntityTypeBuilder<UserMovieWatchList> builder)
        {
            builder.HasKey(umwl => new { umwl.UserId, umwl.TMDBId });            
            builder.HasOne(umwl => umwl.User)
                .WithMany(u => u.WatchListMovies)
                .HasForeignKey(umwl => umwl.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(umwl => umwl.Movie)
                .WithMany(m => m.UserWatchLists)
                .HasForeignKey(umwl => umwl.TMDBId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(umwl => umwl.AddedOn)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
