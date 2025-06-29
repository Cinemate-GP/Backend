﻿using Cinemate.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Repository.Data.Configuration
{
    class UserRateMovieConfigration : IEntityTypeConfiguration<UserRateMovie>
    {
        public void Configure(EntityTypeBuilder<UserRateMovie> builder)
        {

            builder.HasKey(urm => new { urm.UserId, urm.TMDBId });            
            builder.HasOne(urm => urm.User)
                .WithMany(u => u.RatedMovies)
                .HasForeignKey(urm => urm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(urm => urm.Movie)
                .WithMany(m => m.UserRates)
                .HasForeignKey(urm => urm.TMDBId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(urm => urm.Stars)
                .HasDefaultValue(null);

            builder.Property(urm => urm.RatedOn)
                .HasDefaultValueSql("GETDATE()");

        }
    }
}
