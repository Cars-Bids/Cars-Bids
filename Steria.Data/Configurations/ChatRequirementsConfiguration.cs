using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Steria.Core.Entities;

namespace Steria.Data.Configurations;

public class ChatRequirementsConfiguration : IEntityTypeConfiguration<ChatRequirements>
{
    public void Configure(EntityTypeBuilder<ChatRequirements> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Text)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.Chat)
            .WithMany(x => x.ChatRequirements)
            .HasForeignKey(x => x.ChatId);

        builder.HasOne(x => x.User)
            .WithMany(x => x.ChatRequirements)
            .HasForeignKey(x => x.CreatedById);
    }
}