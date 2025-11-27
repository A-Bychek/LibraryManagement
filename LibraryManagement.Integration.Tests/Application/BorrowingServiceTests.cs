using LibraryManagement.Application.DTOs.Borrowings;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Contract.Commands.Borrowing;
using LibraryManagement.Contract.QueryModels.Borrowings;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Integration.Tests.Fixtures;
using LibraryManagement.Shared;
using LibraryManagement.Shared.Exceptions;
using SimpleInjector.Lifestyles;

namespace LibraryManagement.Integration.Tests.Application;

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

    [Fact]
    public async Task ReturnBookAsync_WhenRequestIsValid_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBorrowingService _borrowingService = _fixture.Container.GetInstance<IBorrowingService>();
            ReturnBookCommand returnBookCommand = new ReturnBookCommand
            {
                BorrowingId = 2,
            };

            BorrowingDto borrowing = await _borrowingService.ReturnBookAsync(returnBookCommand);

            Assert.NotNull(borrowing);
            Assert.Equal(2, borrowing.BorrowingId);
            Assert.Equal(2, borrowing.BookId);
            Assert.Equal("Returned", borrowing.Status);
            Assert.True(borrowing.FineAmount.Value > 0);
        }
    }

    [Fact]
    public async Task ReturnBookAsync_WhenBorrowingIdDoesnotExist_ShouldThrowNotFoundException()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBorrowingService _borrowingService = _fixture.Container.GetInstance<IBorrowingService>();
            ReturnBookCommand returnBookCommand = new ReturnBookCommand
            {
                BorrowingId = 22,
            };

            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await _borrowingService.ReturnBookAsync(returnBookCommand);
            });
        }
    }

    [Fact]
    public async Task GetUserBorrowingsAsync_WhenRequestIsValid_ShouldReturnEntities()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBorrowingService _borrowingService = _fixture.Container.GetInstance<IBorrowingService>();
            BorrowingSearchArgs borrowingSearchArgs = new BorrowingSearchArgs
            {
                UserId = 1,
                Status = BorrowingStatus.Returned,
                PageNumber = 1,
                PageSize = 15
            };

            PagedResult<BorrowingDto>? userBorrowings = await _borrowingService.GetUserBorrowingsAsync(borrowingSearchArgs);

            Assert.NotNull(userBorrowings);
            Assert.Equal(1, userBorrowings.TotalCount);
        }
    }

    [Fact]
    public async Task GetUserBorrowingsAsync_WhenBorrowingIdDoesnotExist_ShouldThrowNotFoundException()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBorrowingService _borrowingService = _fixture.Container.GetInstance<IBorrowingService>();
            BorrowingSearchArgs borrowingSearchArgs = new BorrowingSearchArgs
            {
                UserId = 22,
                Status = BorrowingStatus.Overdue,
                PageNumber = 1,
                PageSize = 15
            };

            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await _borrowingService.GetUserBorrowingsAsync(borrowingSearchArgs);
            });
        }
    }

    [Fact]
    public async Task GetOverdueBooksAsync_WhenRequestIsValid_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBorrowingService _borrowingService = _fixture.Container.GetInstance<IBorrowingService>();

            List<BorrowingDto>? overdueBorrowings = await _borrowingService.GetOverdueBooksAsync();

            Assert.NotNull(overdueBorrowings);
            Assert.Equal(2, overdueBorrowings.Count);
        }
    }

    [Fact]
    public async Task CalculateFineAsync_WhenRequestIsValid_ShouldReturnFineAmount()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBorrowingService _borrowingService = _fixture.Container.GetInstance<IBorrowingService>();

            double fineAmount = await _borrowingService.CalculateFineAsync(2);

            Assert.True(fineAmount > 0);
        }
    }
}
