using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Data.Configurations.Authors;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> modelBuilder)
    {
        modelBuilder.ToTable("Authors");

        modelBuilder.HasKey(x => x.AuthorId);

        modelBuilder.Property(x => x.AuthorId)
            .ValueGeneratedOnAdd();

        modelBuilder.Property(x => x.FirstName)
            .HasMaxLength(200)
            .IsRequired();

        modelBuilder.Property(x => x.LastName)
            .HasMaxLength(200)
            .IsRequired();

        modelBuilder.Property(x => x.Biography)
            .HasMaxLength(2000);

        modelBuilder.Property(x => x.IsActive)
            .HasDefaultValue(true);
    }
}


