using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarsAndBids.Data.Entities.Configurations;

public class AuctionConfiguration : IEntityTypeConfiguration<Auction>
{
    public void Configure(EntityTypeBuilder<Auction> builder)
    {
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.StartPrice)
            .HasColumnType("decimal(10,2)");
        
        builder.Property(a => a.CurrentPrice)
            .HasColumnType("decimal(10,2)");
        
        builder.Property(a => a.Status)
            .HasConversion<string>()
            .HasMaxLength(20);
        
        builder.Property(a => a.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        builder.HasOne(a => a.Car)
            .WithMany(c => c.Auctions)
            .HasForeignKey(a => a.CarId);
        
        builder.HasOne(a => a.Seller)
            .WithMany(u => u.Auctions)
            .HasForeignKey(a => a.SellerId);
    }
}