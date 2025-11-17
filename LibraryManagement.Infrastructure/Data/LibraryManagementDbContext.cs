using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infrastructure.Data
{
    public class LibraryManagementDbContext : DbContext
    {

        public DbSet<Book> Books { get; set; } = null!;

        public DbSet<Author> Authors { get; set; } = null!;

        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<Borrowing> Borrowings { get; set; } = null!;

        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryManagementDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
