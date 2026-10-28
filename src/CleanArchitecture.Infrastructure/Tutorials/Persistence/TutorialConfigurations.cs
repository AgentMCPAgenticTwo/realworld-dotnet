using CleanArchitecture.Domain.Categories;
using CleanArchitecture.Domain.Tutorials;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Tutorials.Persistence;

public class TutorialConfigurations : IEntityTypeConfiguration<Tutorial>
{
    public void Configure(EntityTypeBuilder<Tutorial> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedNever();

        builder.Property(t => t.Title)
            .HasMaxLength(200);

        builder.Property(t => t.Description);

        builder.Property(t => t.Published);

        builder.Property(t => t.CategoryId);

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(t => t.CreatedAt);

        builder.Property(t => t.UpdatedAt);
    }
}
