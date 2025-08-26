using Steria.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Steria.Data.Configurations;

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.AnswerText)
            .HasMaxLength(300)
            .HasColumnType("text")
            .IsRequired();
        
        builder.Property(a => a.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        builder.HasOne(a => a.Question)
            .WithOne(q => q.Answer)
            .HasForeignKey<Answer>(a => a.QuestionId);
        
        builder.HasOne(a => a.User)
            .WithMany(u => u.Answers)
            .HasForeignKey(a => a.UserId);
    }
}