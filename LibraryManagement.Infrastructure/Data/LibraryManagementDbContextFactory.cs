using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LibraryManagement.Infrastructure.Data;

public class LibraryManagementDbContextFactory : IDesignTimeDbContextFactory<LibraryManagementDbContext>
{
    public LibraryManagementDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? "Data Source=test.db";

        var optionsBuilder = new DbContextOptionsBuilder<LibraryManagementDbContext>();
        optionsBuilder.UseSqlite(connectionString);
        return new LibraryManagementDbContext(optionsBuilder.Options);
    }
}
