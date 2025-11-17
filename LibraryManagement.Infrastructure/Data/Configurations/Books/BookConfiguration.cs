using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Data.Configurations.Books;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> modelBuilder)
    {
        modelBuilder.ToTable("Books");

        modelBuilder.HasKey(x => x.BookId);

        modelBuilder.Property(x => x.BookId)
            .ValueGeneratedOnAdd();

        modelBuilder.Property(x => x.Title)
            .HasMaxLength(200)
            .IsRequired();

        modelBuilder.Property(x => x.ISBN)
            .HasMaxLength(13);
        modelBuilder.HasIndex(x => x.ISBN)
            .IsUnique();

        modelBuilder.Property(x => x.Description)
            .HasMaxLength(2000);

        modelBuilder.Property(x => x.IsAvailable)
            .HasDefaultValue(true);

        modelBuilder.Property(x => x.CreatedDate)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.HasOne(x => x.Author)
            .WithMany(y => y.Books)
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.HasOne(x => x.Category)
            .WithMany(c => c.Books)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);


        modelBuilder.HasIndex(x => x.ISBN);
        modelBuilder.HasIndex(x => x.AuthorId);
        modelBuilder.HasIndex(x => x.CategoryId);
        modelBuilder.HasIndex(x => x.IsAvailable);
    }
}


