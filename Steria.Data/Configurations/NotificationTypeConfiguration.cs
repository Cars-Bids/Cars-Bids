using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Steria.Core.Entities;

namespace Steria.Data.Configurations;

public class NotificationTypeConfiguration : IEntityTypeConfiguration<NotificationType>
{
    public void Configure(EntityTypeBuilder<NotificationType> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Key)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(n => n.RedirectRoute)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(n => n.Description)
            .HasMaxLength(100);
        
        builder.Property(n => n.SourceType)
            .HasConversion<string>()
            .HasMaxLength(30);
    }
}