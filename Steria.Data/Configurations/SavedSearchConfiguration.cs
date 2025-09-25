using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Steria.Core.Entities;

namespace Steria.Data.Configurations;

public class SavedSearchConfiguration : IEntityTypeConfiguration<SavedSearch>
{
    public void Configure(EntityTypeBuilder<SavedSearch> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasOne(x => x.Make)
            .WithMany(x => x.SavedSearches)
            .HasForeignKey(x => x.MakeId);

        builder.HasOne(x => x.Model)
            .WithMany(x => x.SavedSearches)
            .HasForeignKey(x => x.ModelId);

        builder.HasOne(x => x.User)
            .WithMany(x => x.SavedSearches)
            .HasForeignKey(x => x.UserId);
    }
}