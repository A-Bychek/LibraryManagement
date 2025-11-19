using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Data.Configurations.Borrowings;

public class BorrowingConfiguration : IEntityTypeConfiguration<Borrowing>
{
    public void Configure(EntityTypeBuilder<Borrowing> modelBuilder)
    {
        modelBuilder.ToTable("Borrowings");

        modelBuilder.HasKey(x => x.BorrowingId);

        modelBuilder.Property(x => x.BorrowingId)
            .ValueGeneratedOnAdd();

        modelBuilder.Property(x => x.BorrowDate)
            .IsRequired();

        modelBuilder.Property(x => x.DueDate)
            .IsRequired();

        modelBuilder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(15);

        modelBuilder.HasOne(x => x.Book)
            .WithMany()
            .HasForeignKey(x => x.BookId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.HasOne(x => x.User)
            .WithOne(y => y.Borrowing)
            .HasForeignKey<Borrowing>(x => x.UserId);

        modelBuilder.HasIndex(x => x.UserId);
        modelBuilder.HasIndex(x => x.Status);
        modelBuilder.HasIndex(x => x.DueDate);
    }
}


