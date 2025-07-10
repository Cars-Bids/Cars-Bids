using CarsAndBids.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarsAndBids.Data.Configurations;

public class MakeConfiguration : IEntityTypeConfiguration<Make>
{
    public void Configure(EntityTypeBuilder<Make> builder)
    {
        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.Name)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.HasIndex(m => m.Name)
            .IsUnique();
    }
}