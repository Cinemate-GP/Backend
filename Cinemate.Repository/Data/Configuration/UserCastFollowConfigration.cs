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
    class UserCastFollowConfigration : IEntityTypeConfiguration<UserCastFollow>
    {
        public void Configure(EntityTypeBuilder<UserCastFollow> builder)
        {
            builder.HasKey(uc => new { uc.UserId, uc.CastId });

            builder.HasOne(uc => uc.Cast)
                .WithMany(c => c.FollowedByUsers)
                .HasForeignKey(uc => uc.CastId);

            builder.HasOne(uc => uc.User)
                .WithMany(u => u.FollowedCasts)
                .HasForeignKey(uc => uc.UserId);

            builder.Property(uc => uc.FollowDate)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
