using Steria.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Steria.Data.Configurations;

public class CarImageConfiguration : IEntityTypeConfiguration<CarImage>
{
    public void Configure(EntityTypeBuilder<CarImage> builder)
    {
        builder.HasKey(ci => ci.Id);
        
        builder.Property(ci => ci.ImageUrl)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(ci => ci.ImageCategory)
            .HasConversion<string>()
            .HasMaxLength(50);
        
        builder.Property(ci => ci.UploadedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        builder.HasOne(ci => ci.Car)
            .WithMany(c => c.Images)
            .HasForeignKey(ci => ci.CarId);
    }
}