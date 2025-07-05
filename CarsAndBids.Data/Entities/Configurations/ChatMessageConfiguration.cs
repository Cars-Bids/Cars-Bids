using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarsAndBids.Data.Entities.Configurations;

public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.HasKey(cm => cm.Id);
        
        builder.Property(cm => cm.Message)
            .HasColumnType("text")
            .IsRequired();
        
        builder.Property(cm => cm.HasAttachments)
            .HasDefaultValue(false);
        
        builder.Property(cm => cm.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        builder.HasOne(cm => cm.Chat)
            .WithMany(c => c.Messages)
            .HasForeignKey(cm => cm.ChatId);
        
        builder.HasOne(cm => cm.User)
            .WithMany(u => u.ChatMessages)
            .HasForeignKey(cm => cm.SenderId);
    }
}