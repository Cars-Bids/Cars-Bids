using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarsAndBids.Data.Entities.Configurations;

public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
{
    public void Configure(EntityTypeBuilder<Wishlist> builder)
    {
        builder.HasKey(w => w.Id);
        
        builder.Property(w => w.AddedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        builder.HasOne(w => w.User)
            .WithMany(u => u.Wishlists)
            .HasForeignKey(w => w.UserId);
        
        builder.HasOne(w => w.Auction)
            .WithMany(a => a.Wishlists)
            .HasForeignKey(w => w.AuctionId);
        
        builder.HasIndex(w => new { w.UserId, w.AuctionId })
            .IsUnique();
    }
}