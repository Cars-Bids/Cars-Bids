using CarsAndBids.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarsAndBids.Data.Configurations;

public class EmojiReactionConfiguration : IEntityTypeConfiguration<EmojiReaction>
{
    public void Configure(EntityTypeBuilder<EmojiReaction> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Emoji)
               .HasMaxLength(15);

        builder.HasOne(x => x.UserChatMessageReaction)
            .WithMany(x => x.EmojiReactions)
            .HasForeignKey(x => x.MessageReactionId);
    }
}