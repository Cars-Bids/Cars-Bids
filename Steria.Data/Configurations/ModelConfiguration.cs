using Steria.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarsAndBids.Data.Configurations;

public class ModelConfiguration : IEntityTypeConfiguration<Model>
{
    public void Configure(EntityTypeBuilder<Model> builder)
    {
        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.Name)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.HasIndex(m => m.Name)
            .IsUnique();
        
        builder.HasOne(m => m.Make)
            .WithMany(ma => ma.Models)
            .HasForeignKey(m => m.MakeId);
    }
}