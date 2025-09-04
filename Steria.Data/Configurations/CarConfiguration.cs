using Steria.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Steria.Data.Configurations;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Vin)
            .HasMaxLength(17)
            .IsRequired();

        builder.HasIndex(c => c.Vin)
            .IsUnique();

        builder.Property(c => c.Highlights)
            .HasColumnType("text");

        builder.Property(c => c.ServiceHistory)
            .HasColumnType("text");

        builder.Property(c => c.Equipment)
            .HasColumnType("text");

        builder.Property(c => c.Flaws)
            .HasColumnType("text");

        builder.Property(c => c.Modifications)
            .HasColumnType("text");

        builder.Property(c => c.OtherItems)
            .HasColumnType("text");

        builder.Property(c => c.OwnershipHistory)
            .HasColumnType("text");

        builder.Property(c => c.SellerNotes)
            .HasColumnType("text");

        builder.Property(c => c.VideoLinks)
            .HasColumnType("text");

        builder.Property(c => c.ExteriorColor)
            .HasMaxLength(50);

        builder.Property(c => c.InteriorColor)
            .HasMaxLength(50);

        builder.Property(c => c.Location)
            .HasMaxLength(100);

        builder.Property(c => c.Drivetrain)
            .HasConversion<string>()
            .HasMaxLength(10);

        builder.Property(c => c.Engine)
            .HasMaxLength(50);

        builder.Property(c => c.TransmissionType)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(c => c.Status)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(c => c.Manager)
            .WithMany(u => u.AssingCars)
            .HasForeignKey(c => c.ManagerId);

        builder.HasOne(c => c.Owner)
            .WithMany(u => u.OwnedCars)
            .HasForeignKey(c => c.OwnerId);

        builder.HasOne(c => c.BodyStyle)
            .WithMany(bs => bs.Cars)
            .HasForeignKey(c => c.BodyStyleId);

        builder.HasOne(c => c.Model)
            .WithMany(m => m.Cars)
            .HasForeignKey(c => c.ModelId);

        builder.HasOne(c => c.Chat)
            .WithOne(c => c.Car)
            .HasForeignKey<Car>(c => c.ChatId);
    }
}