using Steria.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Steria.Data.Configurations;

public class ChatAttachmentConfiguration : IEntityTypeConfiguration<ChatAttachment>
{
    public void Configure(EntityTypeBuilder<ChatAttachment> builder)
    {
        builder.HasKey(ca => ca.Id);
        
        builder.Property(ca => ca.ImageUrl)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(ca => ca.UploadedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        builder.HasOne(ca => ca.Message)
            .WithMany(cm => cm.Attachments)
            .HasForeignKey(ca => ca.MessageId);
    }
}