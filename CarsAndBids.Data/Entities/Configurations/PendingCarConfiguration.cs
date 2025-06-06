using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarsAndBids.Data.Entities.Configurations;

public class PendingCarConfiguration : IEntityTypeConfiguration<PendingCar>
{
    public void Configure(EntityTypeBuilder<PendingCar> builder)
    {
        builder.HasKey(pc => pc.Id);
        
        builder.Property(pc => pc.Vin)
            .HasMaxLength(17)
            .IsRequired();
        
        builder.HasIndex(pc => pc.Vin)
            .IsUnique();
        
        builder.Property(pc => pc.ExteriorColor)
            .HasMaxLength(50);
        
        builder.Property(pc => pc.InteriorColor)
            .HasMaxLength(50);
        
        builder.Property(pc => pc.Location)
            .HasMaxLength(100);
        
        builder.Property(pc => pc.Drivetrain)
            .HasConversion<string>()
            .HasMaxLength(10);
        
        builder.Property(pc => pc.Engine)
            .HasMaxLength(50);
        
        builder.Property(pc => pc.TransmissionType)
            .HasConversion<string>()
            .HasMaxLength(50);
        
        builder.Property(pc => pc.Modifications)
            .HasColumnType("text");
        
        builder.Property(pc => pc.Flaws)
            .HasColumnType("text");
        
        builder.Property(pc => pc.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        builder.HasOne(pc => pc.Owner)
            .WithMany(u => u.PendingCars)
            .HasForeignKey(pc => pc.OwnerId);
        
        builder.HasOne(pc => pc.Model)
            .WithMany(m => m.PendingCars)
            .HasForeignKey(pc => pc.ModelId);
        
        builder.HasOne(pc => pc.BodyStyle)
            .WithMany(bs => bs.PendingCars)
            .HasForeignKey(pc => pc.BodyStyleId);
    }
}