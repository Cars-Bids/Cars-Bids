using CarsAndBids.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarsAndBids.Data.Configurations;

public class BodyStyleConfiguration : IEntityTypeConfiguration<BodyStyle>
{
    public void Configure(EntityTypeBuilder<BodyStyle> builder)
    {
        builder.HasKey(bs => bs.Id);
        
        builder.Property(bs => bs.StyleName)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.HasIndex(bs => bs.StyleName)
            .IsUnique();
    }
}