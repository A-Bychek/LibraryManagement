using FluentValidation;
using LibraryManagement.Application.Commands.Author;
using LibraryManagement.Application.DTOs.Authors;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Application.QueryModels.Authors;
using LibraryManagement.Integration.Tests.Fixtures;
using LibraryManagement.Shared;
using LibraryManagement.Shared.Exceptions;
using SimpleInjector.Lifestyles;

namespace LibraryManagement.Integration.Tests.Infrastructure;

public class AuthorServiceTests : IClassFixture<SqliteTestDatabaseFixture>
{
    private readonly SqliteTestDatabaseFixture _fixture;
    public AuthorServiceTests(SqliteTestDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetAuthorAsync_WhenIdExists_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IAuthorService _authorService = _fixture.Container.GetInstance<IAuthorService>();

            AuthorDto? author = await _authorService.GetAuthorAsync(1);

            Assert.NotNull(author);
            Assert.Equal(1, author.AuthorId);
            Assert.Equal("Test Name 1", author.FirstName);
            Assert.Equal("Last Name 1", author.LastName);
            Assert.Equal("1/1/1950 12:00:00 AM", author.DateOfBirth);
        }
    }
    
    [Fact]
    public async Task GetAuthorAsync_WhenIdDoesnotExist_ShouldThrowsNotFoundException()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IAuthorService _authorService = _fixture.Container.GetInstance<IAuthorService>();

            await Assert.ThrowsAsync<NotFoundException>(async () => await _authorService.GetAuthorAsync(111));
        }
    }

    [Fact]
    public async Task GetAuthorsAsync_IfRequestIsValid_ShouldReturnEntities()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IAuthorService _authorService = _fixture.Container.GetInstance<IAuthorService>();
            AuthorSearchArgs args = new AuthorSearchArgs
            {
                SearchTerm = "Test",
                IsActive = true,
                PageNumber = 1,
                PageSize = 15
            };

            PagedResult<AuthorDto>? authors = await _authorService.GetAuthorsAsync(args);

            Assert.NotNull(authors);
            Assert.Equal(2, authors.TotalCount);

        }
    }

    [Fact]
    public async Task CreateAuthorAsync_IfRequestIsValid_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IAuthorService _authorService = _fixture.Container.GetInstance<IAuthorService>();
            CreateAuthorCommand createAuthorCommand = new CreateAuthorCommand
            {
                FirstName = "Test Name 4",
                LastName = "Last Name 4",
                DateOfBirth = "2000-01-01"
            };

            AuthorDto? addedAuthor = await _authorService.CreateAuthorAsync(createAuthorCommand);

            Assert.NotNull(addedAuthor);
            Assert.Equal(4, addedAuthor.AuthorId);
            Assert.Equal("Test Name 4", addedAuthor.FirstName);
            Assert.Equal("Last Name 4", addedAuthor.LastName);
            Assert.Equal("1/1/2000 12:00:00 AM", addedAuthor.DateOfBirth);
        }
    }
    
    [Fact]
    public async Task CreateAuthorAsync_IfRequestIsInvalid_ShouldThrowValidationException()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IAuthorService _authorService = _fixture.Container.GetInstance<IAuthorService>();
            CreateAuthorCommand createAuthorCommand = new CreateAuthorCommand();

            await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await _authorService.CreateAuthorAsync(createAuthorCommand);
            });
        }
    }
    
    [Fact]
    public async Task UpdateAuthorAsync_IfRequestIsValid_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IAuthorService _authorService = _fixture.Container.GetInstance<IAuthorService>();
            UpdateAuthorCommand updateAuthorCommand = new UpdateAuthorCommand
            {
                AuthorId = 2,
                FirstName = "Test Name Updated",
                LastName = "Test Last Name Updated"
            };

            await _authorService.UpdateAuthorAsync(updateAuthorCommand);

            var updatedAuthor = await _authorService.GetAuthorAsync(2);

            Assert.NotNull(updatedAuthor);
            Assert.Equal(2, updatedAuthor.AuthorId);
            Assert.Equal("Test Name Updated", updatedAuthor.FirstName);
            Assert.Equal("Test Last Name Updated", updatedAuthor.LastName);
            Assert.Equal("1/1/1970 12:00:00 AM", updatedAuthor.DateOfBirth);
        }
    }

    [Fact]
    public async Task UpdateAuthorAsync_IfRequestIsInvalid_ShouldThrowValidationException()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IAuthorService _authorService = _fixture.Container.GetInstance<IAuthorService>();
            UpdateAuthorCommand updateAuthorCommand = new UpdateAuthorCommand();

            await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await _authorService.UpdateAuthorAsync(updateAuthorCommand);
            });
        }
    }

    [Fact]
    public async Task GetAuthorBookCountAsync_IfRequestIsValid_ShouldReturnBookCount()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IAuthorService _authorService = _fixture.Container.GetInstance<IAuthorService>();
            
            await _authorService.GetAuthorBookCountAsync(2);

            int? bookCount = await _authorService.GetAuthorBookCountAsync(2);

            Assert.NotNull(bookCount);
            Assert.Equal(2, bookCount);
        }
    }

    [Fact]
    public async Task GetAuthorBookCountAsync_IfAuthorDoesnotExist_ShouldThrowNotFoundException()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IAuthorService _authorService = _fixture.Container.GetInstance<IAuthorService>();

            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await _authorService.GetAuthorBookCountAsync(11);
            });
        }
    }
    
    [Fact]
    public async Task DeleteAuthorAsync_IfRequestIsValid_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IAuthorService _authorService = _fixture.Container.GetInstance<IAuthorService>();

            DeleteAuthorDto deletedAuthor = await _authorService.DeleteAuthorAsync(3);

            Assert.NotNull(deletedAuthor);
            Assert.Equal(true, deletedAuthor.Success);
            Assert.Equal("Successfully removed the author with 3 authorId.", deletedAuthor.Message);
        }
    }

    [Fact]
    public async Task DeleteAuthorAsync_IfAuthorDoesnotExist_ShouldNotFoundException()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IAuthorService _authorService = _fixture.Container.GetInstance<IAuthorService>();

            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await _authorService.DeleteAuthorAsync(111);
            });
        }
    }
}
