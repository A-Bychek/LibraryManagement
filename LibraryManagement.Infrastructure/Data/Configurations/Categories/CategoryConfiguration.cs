using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Data.Configurations.Categories;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> modelBuilder)
    {
        modelBuilder.ToTable("Categories");

        modelBuilder.HasKey(x => x.CategoryId);

        modelBuilder.Property(x => x.CategoryId)
            .ValueGeneratedOnAdd();

        modelBuilder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();
        modelBuilder.HasIndex(x => x.Name)
            .IsUnique();

        modelBuilder.Property(x => x.Description)
            .HasMaxLength(2000);

        modelBuilder.Property(x => x.SortOrder)
            .HasDefaultValue(0);

        modelBuilder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        modelBuilder.HasOne(x => x.ParentCategory)
            .WithMany(x => x.SubCategories)
            .HasForeignKey(x => x.ParentCategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.HasIndex(x => x.ParentCategoryId);
    }
}


