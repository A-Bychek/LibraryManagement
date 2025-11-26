using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.QueryModels.Authors;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Integration.Tests.Fixtures;
using LibraryManagement.Shared;
using Microsoft.EntityFrameworkCore;
using SimpleInjector.Lifestyles;

namespace LibraryManagement.Integration.Tests.Infrastructure;

public class AuthorRepositoryTests : IClassFixture<SqliteTestDatabaseFixture>
{
    private readonly SqliteTestDatabaseFixture _fixture;
    public AuthorRepositoryTests(SqliteTestDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetByIdAsync_WhenIdExists_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IAuthorRepository _authorRepository = _fixture.Container.GetInstance<IAuthorRepository>();

            Author? author = await _authorRepository.GetByIdAsync(1);

            Assert.NotNull(author);
            Assert.Equal(1, author.AuthorId);
            Assert.Equal("Test Name 1", author.FirstName);
            Assert.Equal("Last Name 1", author.LastName);
            Assert.Equal(new DateTime(1950,01,01), author.DateOfBirth);
        }
    }

    [Fact]
    public async Task GetByIdAsync_WhenIdDoesnotExist_ShouldReturnNull()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IAuthorRepository _authorRepository = _fixture.Container.GetInstance<IAuthorRepository>();

            Author? author = await _authorRepository.GetByIdAsync(111);
            Assert.Null(author);
        }
    }

    [Fact]
    public async Task AddAsync_IfRequestIsValid_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IAuthorRepository _authorRepository = _fixture.Container.GetInstance<IAuthorRepository>();
            Author author = new Author
            {
                FirstName = "Test Name 4",
                LastName = "Last Name 4",
                Biography = "Test Biography 4",
                DateOfBirth = new DateTime(1999, 04, 04),
                IsActive = true
            };

            Author? addedAuthor = await _authorRepository.AddAsync(author);

            Assert.NotNull(addedAuthor);
            Assert.Equal(4, addedAuthor.AuthorId);
            Assert.Equal("Test Name 4", addedAuthor.FirstName);
            Assert.Equal("Last Name 4", addedAuthor.LastName);
            Assert.Equal(new DateTime(1999, 04, 04), addedAuthor.DateOfBirth);
        }
    }

    [Fact]
    public async Task AddAsync_IfRequestIsInvalid_ShouldThrowDbUpdateException()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IAuthorRepository _authorRepository = _fixture.Container.GetInstance<IAuthorRepository>();
            LibraryManagementDbContext context = _fixture.Container.GetInstance<LibraryManagementDbContext>();
            Author author = new Author();

            await Assert.ThrowsAsync<DbUpdateException>(async () =>
            {
                await _authorRepository.AddAsync(author);
                await context.SaveChangesAsync();
            });
        }
    }

    [Fact]
    public async Task UpdateAsync_IfRequestIsValid_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IAuthorRepository _authorRepository = _fixture.Container.GetInstance<IAuthorRepository>();
            LibraryManagementDbContext context = _fixture.Container.GetInstance<LibraryManagementDbContext>();
            Author author = new Author
            {
                AuthorId = 2,
                FirstName = "Test Name Updated",
                LastName = "Last Name Updated",
                DateOfBirth = new DateTime(2000, 03, 03)
            };

            await _authorRepository.UpdateAsync(author);
            await context.SaveChangesAsync();

            var updatedAuthor = await _authorRepository.GetByIdAsync(2);

            Assert.NotNull(updatedAuthor);
            Assert.Equal(2, updatedAuthor.AuthorId);
            Assert.Equal("Test Name Updated", updatedAuthor.FirstName);
            Assert.Equal("Last Name Updated", updatedAuthor.LastName);
            Assert.Equal(new DateTime(2000, 03, 03), updatedAuthor.DateOfBirth);
        }
    }

    [Fact]
    public async Task UpdateAsync_IfRequestIsInvalid_ShouldThrowDbUpdateException()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IAuthorRepository _authorRepository = _fixture.Container.GetInstance<IAuthorRepository>();
            LibraryManagementDbContext context = _fixture.Container.GetInstance<LibraryManagementDbContext>();
            Author author = new Author();

            await Assert.ThrowsAsync<DbUpdateException>(async () =>
            {
                await _authorRepository.UpdateAsync(author);
                await context.SaveChangesAsync();
            });
        }
    }
    
    [Fact]
    public async Task DeleteAsync_IfRequestIsValid_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IAuthorRepository _authorRepository = _fixture.Container.GetInstance<IAuthorRepository>();
            LibraryManagementDbContext context = _fixture.Container.GetInstance<LibraryManagementDbContext>();
            Author author = new Author
            {
                AuthorId = 3
            };

            await _authorRepository.DeleteAsync(author);
            await context.SaveChangesAsync();

            var deletedAuthor = await _authorRepository.GetByIdAsync(3);

            Assert.Null(deletedAuthor);
        }
    }

    [Fact]
    public async Task DeleteAsync_IfRequestIsInvalid_ShouldThrowDbUpdateException()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IAuthorRepository _authorRepository = _fixture.Container.GetInstance<IAuthorRepository>();
            LibraryManagementDbContext context = _fixture.Container.GetInstance<LibraryManagementDbContext>();
            Author author = new Author
            {
                AuthorId = 3,
                FirstName = "Test Name 1",
            };

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            {
                await _authorRepository.DeleteAsync(author);
                await context.SaveChangesAsync();
            });
        }
    }

    [Fact]
    public async Task GetAllAsync_WhenIdsExist_ShouldReturnEntities()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IAuthorRepository _authorRepository = _fixture.Container.GetInstance<IAuthorRepository>();

            ICollection<Author> authors = await _authorRepository.GetAllAsync();

            Assert.NotNull(authors);
            Assert.Equal(2, authors.Count);
        }
    }

    [Fact]
    public async Task FindAsync_WhenEntityExists_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IAuthorRepository _authorRepository = _fixture.Container.GetInstance<IAuthorRepository>();
            AuthorSearchArgs args = new AuthorSearchArgs
            {
                SearchTerm = "Test",
                IsActive = true,
                PageNumber = 1,
                PageSize = 15
            };

            PagedResult<Author>? results = await _authorRepository.FindAsync(args);

            Assert.NotNull(results);
            Assert.Equal("Test Name 1", results.Items.First().FirstName);
        }
    }

    [Fact]
    public async Task FindAsync_WhenEntityDoesnotExist_ShouldReturnEmptyItems()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IAuthorRepository _authorRepository = _fixture.Container.GetInstance<IAuthorRepository>();
            AuthorSearchArgs args = new AuthorSearchArgs
            {
                SearchTerm = "Negative Test",
                IsActive = true,
                PageNumber = 1,
                PageSize = 15
            };

            PagedResult<Author>? results = await _authorRepository.FindAsync(args);

            Assert.NotNull(results);
            Assert.Equal([], results.Items);
        }
    }
}
