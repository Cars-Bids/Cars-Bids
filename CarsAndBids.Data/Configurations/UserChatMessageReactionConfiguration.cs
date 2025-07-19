using CarsAndBids.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarsAndBids.Data.Configurations;

public class UserChatMessageReactionConfiguration : IEntityTypeConfiguration<UserChatMessageReaction>
{
    public void Configure(EntityTypeBuilder<UserChatMessageReaction> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.ChatMessage)
            .WithMany(x => x.UserChatMessageReactions)
            .HasForeignKey(x => x.ChatMessageId);

        builder.HasOne(x => x.User)
            .WithMany(x => x.UserChatMessageReactions)
            .HasForeignKey(x => x.UserId);
        
        builder.Property(x => x.SeenAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}