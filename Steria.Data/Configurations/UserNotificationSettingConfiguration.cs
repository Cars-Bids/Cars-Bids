using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Steria.Core.Entities;

namespace Steria.Data.Configurations;

public class UserNotificationSettingConfiguration : IEntityTypeConfiguration<UserNotificationSetting>
{
    public void Configure(EntityTypeBuilder<UserNotificationSetting> builder)
    {
        builder.HasKey(n => n.Id);

        builder.HasOne(n => n.NotificationType)
            .WithMany(n => n.UserNotificationSettings)
            .HasForeignKey(n => n.NotificationTypeId);

        builder.HasOne(n => n.User)
            .WithMany(u => u.UserNotificationSettings)
            .HasForeignKey(n => n.UserId);
    }
}