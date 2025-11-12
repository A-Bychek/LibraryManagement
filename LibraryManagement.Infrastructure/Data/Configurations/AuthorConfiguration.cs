using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Data.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("authors");

        builder.HasKey(x => x.AuthorId);

        builder.Property(x => x.AuthorId)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.FirstName)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.Biography)
            .HasMaxLength(4000);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.Property(x => x.CreatedDate)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(x => new { x.LastName, x.FirstName });
        builder.HasIndex(x => x.IsActive);
    }
}


