using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using SimpleInjector;

namespace LibraryManagement.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this Container container, DbContextOptions<LibraryManagementDbContext> dbOptions)
    {
        container.Register(() => new LibraryManagementDbContext(dbOptions), Lifestyle.Scoped);
        container.Register<IAuthorRepository, AuthorRepository>(Lifestyle.Scoped);
        container.Register<IBookRepository, BookRepository>(Lifestyle.Scoped);
        container.Register<IBorrowingRepository, BorrowingRepository>(Lifestyle.Scoped);
        container.Register<ICategoryRepository, CategoryRepository>(Lifestyle.Scoped);
    }
}