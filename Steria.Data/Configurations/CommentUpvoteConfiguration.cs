using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Steria.Core.Entities;

namespace Steria.Data.Configurations;

public class CommentUpvoteConfiguration : IEntityTypeConfiguration<CommentUpvote>
{
    public void Configure(EntityTypeBuilder<CommentUpvote> builder)
    {
        builder.HasKey(x => new { x.UserId, x.CommentId });

        builder.HasOne(x => x.User)
            .WithMany(x => x.CommentUpvotes)
            .HasForeignKey(x => x.UserId);

        builder.HasOne(x => x.Comment)
            .WithMany(x => x.CommentUpvotes)
            .HasForeignKey(x => x.CommentId);
    }
}