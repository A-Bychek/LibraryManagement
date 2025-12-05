using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace LibraryManagement.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<LibraryManagementDbContext>();
            await SeedData(context);
        }
    }
    public static async Task SeedData(LibraryManagementDbContext dbContext)
    {
        dbContext.Database.EnsureCreatedAsync().Wait();
        await SeedAuthors(dbContext);
        await SeedCategories(dbContext);
        await SeedBooks(dbContext);
        await SeedUsers(dbContext);
        await SeedBorrowings(dbContext);
    }

    public static async Task SeedAuthors(LibraryManagementDbContext dbContext)
    {
        if (dbContext.Authors.Any()) { return; }

        List<Author> authors = new()
        {
            new Author("Stephen", "King", "Horror fiction author", new DateTime(1947, 09, 21), true),
            new Author("J.K.", "Rowling", "Harry Potter fiction author", new DateTime(1965, 07, 31), true),
            new Author("George R.R.", "Martin", "A Song of Ice and Fire fiction author", new DateTime(1948, 09, 20), true),
            new Author("Agatha", "Christie", "Author of detective novels", new DateTime(1890-09-15), true),
            new Author("Isaac", "Asimov", "Science fiction writer", new DateTime(1920-01-02), true),
            new Author("Maxim", "Gorky", "The author of realistic novels", new DateTime(1868-03-28), true)
        };
            await dbContext.Authors.AddRangeAsync(authors);
            await dbContext.SaveChangesAsync();
    }

    public static async Task SeedBooks(LibraryManagementDbContext dbContext)
    {
        if (dbContext.Books.Any()) { return; }
        List<Book> books = new()
        {
            new Book("The Shining", "9780385121675", "A family heads to an isolated hotel for the winter where a sinister presence influences the father into violence.", 1, 6, new DateTime(1977, 01, 28), 447, true, DateTime.UtcNow, null),
            new Book("Harry Potter and the Philosopher's Stone", "9780747532699", "A young wizard discovers his magical heritage on his 11th birthday.", 2, 4, new DateTime(1997, 06, 26), 223, true, DateTime.UtcNow, null),
            new Book("A Game of Thrones", "9780553103540", "Noble families fight for control of the Iron Throne of Westeros.", 3, 5, new DateTime(1996, 08, 01), 694, false, DateTime.UtcNow, null),
            new Book("Murder on the Orient Express", "9780062693662", "Hercule Poirot investigates a murder on a luxury train.", 4, 8, new DateTime(1934, 01, 01), 256, true, DateTime.UtcNow, null),
            new Book("Foundation", "9780553293357", "A scientist creates a foundation to preserve knowledge through the fall of the Galactic Empire.", 5, 3, new DateTime(1951, 06, 01), 255, true, DateTime.UtcNow, null),
            new Book("It", "9780450411434", "Seven adults return to their hometown to confront a nightmare they had first stumbled on as teenagers.", 1, 6, new DateTime(1986, 09, 15), 1138, true, DateTime.UtcNow, null),
            new Book("Harry Potter and the Chamber of Secrets", "9780439064873", "The second year at Hogwarts School of Witchcraft and Wizardry.", 2, 4, new DateTime(1998, 07, 02), 251, true, DateTime.UtcNow, null),
            new Book("A Clash of Kings", "9780553108033", "The second novel in A Song of Ice and Fire.", 3, 5, new DateTime(1998, 11, 16), 761, true, DateTime.UtcNow, null),
            new Book("And Then There Were None", "9780062073488", "Ten people are invited to an isolated island, and are killed one by one.", 4, 8, new DateTime(1939, 11, 06), 272, true, DateTime.UtcNow, null),
            new Book("The Old Woman Izergil", "9798390533352", "The narration is done on behalf of the author and the heroine, the old woman Izergil. It explores the ideas of freedom, the true meaning of life and love.", 6, 7, new DateTime(1892, 09, 12), 45, true, DateTime.UtcNow, null)
        };
        await dbContext.Books.AddRangeAsync(books);
        await dbContext.SaveChangesAsync();

    }

    public static async Task SeedCategories(LibraryManagementDbContext dbContext)
    {
        if (dbContext.Categories.Any()) { return; }
        List<Category> categories = new()
        {
            new Category("Fiction", "Fictional literature", null, 1, true),
            new Category("Non-Fiction", "Non-fictional literature", null, 0, true),
            new Category("Science Fiction", "Sci-fi literature", 1, 1, true),
            new Category("Fantasy", "Fantasy literature", 1, 1, true),
            new Category("Mystery", "Mystery fiction", 1, 1, true),
            new Category("Horror", "Horror fiction", 1, 0, true),
            new Category("Biography", "Biographical literature", 2, 1, true),
            new Category("Detective", "Detective literature", 2, 0, true)
        };
        await dbContext.Categories.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();
    }

    public static async Task SeedUsers(LibraryManagementDbContext dbContext)
    {
        if (dbContext.Users.Any()) { return; }
        List<User> users = new()
        {
            new User(),
            new User(),
            new User(),
            new User(),
        };
        await dbContext.Users.AddRangeAsync(users);
        await dbContext.SaveChangesAsync();
    }

    public static async Task SeedBorrowings(LibraryManagementDbContext dbContext)
    {
        if (dbContext.Borrowings.Any()) { return; }
        List<Borrowing> borrowings = new()
        {
            new Borrowing(3, 1, new DateTime(2025,11,12), new DateTime(2025, 11, 26), null, BorrowingStatus.Returned),
            new Borrowing(1, 2, new DateTime(2025,12,01), new DateTime(2026, 01, 18), null, BorrowingStatus.Active),
            new Borrowing(5, 3, new DateTime(2025,11,15), new DateTime(2025, 11, 29), new DateTime(2025, 11, 28), BorrowingStatus.Overdue)
        };
        await dbContext.Borrowings.AddRangeAsync(borrowings);
        await dbContext.SaveChangesAsync();

    }
}
