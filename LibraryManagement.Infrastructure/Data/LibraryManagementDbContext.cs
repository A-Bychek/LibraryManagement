using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Data;

public class LibraryManagementDbContext : DbContext
{
    public LibraryManagementDbContext(DbContextOptions<LibraryManagementDbContext> options)
        : base(options) { }

    public DbSet<Book> Books { get; set; } = null!;

    public DbSet<Author> Authors { get; set; } = null!;

    public DbSet<Category> Categories { get; set; } = null!;

    public DbSet<Borrowing> Borrowings { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryManagementDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
