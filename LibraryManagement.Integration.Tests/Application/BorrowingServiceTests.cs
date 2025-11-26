using LibraryManagement.Application.Commands.Borrowing;
using LibraryManagement.Application.DTOs.Borrowings;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Integration.Tests.Fixtures;
using LibraryManagement.Shared.Exceptions;
using SimpleInjector.Lifestyles;

namespace LibraryManagement.Integration.Tests.Infrastructure;

public class BorrowingServiceTests : IClassFixture<SqliteTestDatabaseFixture>
{
    private readonly SqliteTestDatabaseFixture _fixture;
    public BorrowingServiceTests(SqliteTestDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task BorrowBookAsync_WhenIdExists_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBorrowingService _borrowingService = _fixture.Container.GetInstance<IBorrowingService>();
            BorrowBookCommand borrowBookCommand = new BorrowBookCommand
            {
                BookId = 3,
                UserId = 3,
                DaysToReturn = 10
            };

            BorrowingDto? borrowing = await _borrowingService.BorrowBookAsync(borrowBookCommand);

            Assert.NotNull(borrowing);
            Assert.Equal(3, borrowing.BorrowingId);
            Assert.Equal(3, borrowing.BookId);
            Assert.Equal("Test Book 3", borrowing.BookTitle);
        }
    }

    [Fact]
    public async Task BorrowBookAsync_WhenBookIsAlreadyBorrowed_ShouldThrowNotAvailableException()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBorrowingService _borrowingService = _fixture.Container.GetInstance<IBorrowingService>();
            BorrowBookCommand borrowBookCommand = new BorrowBookCommand
            {
                BookId = 2,
                UserId = 3,
                DaysToReturn = 10
            };

            await Assert.ThrowsAsync<NotAvailableException>(async () =>
            {
                await _borrowingService.BorrowBookAsync(borrowBookCommand);
            });
        }
    }
}
