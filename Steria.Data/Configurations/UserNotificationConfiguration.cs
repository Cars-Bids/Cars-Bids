using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Interfaces;
using Steria.Core.Notifications_Custom_Data;

namespace Steria.Data.Configurations;

public class UserNotificationConfiguration : IEntityTypeConfiguration<UserNotification>
{
    public void Configure(EntityTypeBuilder<UserNotification> builder)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        builder.HasKey(n => n.Id);

        builder.HasOne(n => n.User)
            .WithMany(u => u.UserNotifications)
            .HasForeignKey(n => n.UserId);

        builder.HasOne(n => n.NotificationType)
            .WithMany(n => n.UserNotifications)
            .HasForeignKey(n => n.NotificationTypeId);
        
        builder.Property(x => x.CustomDataJson)
            .HasColumnType("jsonb");
        
        builder.Property(n => n.IsRead)
            .IsRequired();
        
        builder.Property(n => n.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
    
}