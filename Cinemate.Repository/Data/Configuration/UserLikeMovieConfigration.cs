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
    class UserLikeMovieConfigration : IEntityTypeConfiguration<UserLikeMovie>
    {
        public void Configure(EntityTypeBuilder<UserLikeMovie> builder)
        {
            builder.HasKey(ulm => new { ulm.UserId, ulm.TMDBId });

            builder.HasOne(ulm => ulm.User)
                .WithMany(u => u.LikedMovies)
                .HasForeignKey(ulm => ulm.UserId);

            builder.HasOne(ulm => ulm.Movie)
                .WithMany(m => m.UserLikes)
                .HasForeignKey(ulm => ulm.TMDBId);

            builder.Property(ulm => ulm.LikedOn)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
