using Steria.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Steria.Data.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Text)
            .HasColumnType("text")
            .IsRequired();
        
        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        builder.HasOne(c => c.Auction)
            .WithMany(a => a.Comments)
            .HasForeignKey(c => c.AuctionId);
        
        builder.HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId);
        
        builder.HasOne(c => c.ReplyedTo)
            .WithMany(c => c.Replies)
            .HasForeignKey(c => c.ReplyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}