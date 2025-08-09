using Steria.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarsAndBids.Data.Configurations;

public class BidConfiguration : IEntityTypeConfiguration<Bid>
{
    public void Configure(EntityTypeBuilder<Bid> builder)
    {
        builder.HasKey(b => b.Id);
        
        builder.Property(b => b.BidAmount)
            .HasColumnType("decimal(10,2)");
        
        builder.Property(b => b.BidTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        builder.HasOne(b => b.Auction)
            .WithMany(a => a.Bids)
            .HasForeignKey(b => b.AuctionId);
        
        builder.HasOne(b => b.User)
            .WithMany(u => u.Bids)
            .HasForeignKey(b => b.UserId);
    }
}