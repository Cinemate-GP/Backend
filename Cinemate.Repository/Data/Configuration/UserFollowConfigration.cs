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
    class UserFollowConfigration : IEntityTypeConfiguration<UserFollow>
    {
        public void Configure(EntityTypeBuilder<UserFollow> builder)
        {
            builder.HasKey(uf => new { uf.UserId, uf.FollowId });            
            builder.HasOne(uf => uf.Follower)
                   .WithMany(u => u.Following)
                   .HasForeignKey(uf => uf.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(uf => uf.FollowedUser)
                   .WithMany(u => u.Followers)
                   .HasForeignKey(uf => uf.FollowId)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.Property(uf => uf.FollowedOn)
                   .IsRequired().
                   HasDefaultValueSql("GETDATE()");
        }
    }
}
