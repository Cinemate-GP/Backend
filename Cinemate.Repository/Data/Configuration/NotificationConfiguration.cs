using Cinemate.Core.Entities;
using Cinemate.Core.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinemate.Repository.Data.Configuration
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Message)
                .IsRequired();

            builder.Property(n => n.NotificationType)
                .IsRequired();

          

            builder.Property(n => n.IsRead)
                .HasDefaultValue(false);

            builder.Property(n => n.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");            
            builder.HasOne(ulm => ulm.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(ulm => ulm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Add relationships

        }
    }
}