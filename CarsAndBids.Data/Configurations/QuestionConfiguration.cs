using CarsAndBids.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarsAndBids.Data.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasKey(q => q.Id);
        
        builder.Property(q => q.QuestionText)
            .HasColumnType("text")
            .IsRequired();
        
        builder.Property(q => q.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        builder.HasOne(q => q.Auction)
            .WithMany(a => a.Questions)
            .HasForeignKey(q => q.AuctionId);
        
        builder.HasOne(q => q.User)
            .WithMany(u => u.Questions)
            .HasForeignKey(q => q.UserId);
    }
}