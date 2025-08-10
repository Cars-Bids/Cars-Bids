using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Steria.Core.Entities;

namespace Steria.Data.Configurations;

public class UserNotificationConfiguration : IEntityTypeConfiguration<UserNotification>
{
    public void Configure(EntityTypeBuilder<UserNotification> builder)
    {
        builder.HasKey(n => n.Id);

        builder.HasOne(n => n.User)
            .WithMany(u => u.UserNotifications)
            .HasForeignKey(n => n.UserId);

        builder.HasOne(n => n.NotificationType)
            .WithMany(n => n.UserNotifications)
            .HasForeignKey(n => n.NotificationTypeId);

        builder.Property(n => n.SourceType)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(n => n.CustomJsonData)
            .HasMaxLength(2000);

        builder.Property(n => n.IsRead)
            .IsRequired();
        
        builder.Property(n => n.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}