using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Integration.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using SimpleInjector.Lifestyles;

namespace LibraryManagement.Integration.Tests.Infrastructure;

public class BorrowingRepositoryTests : IClassFixture<SqliteTestDatabaseFixture>
{
    private readonly SqliteTestDatabaseFixture _fixture;
    public BorrowingRepositoryTests(SqliteTestDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetByIdAsync_WhenIdExists_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBorrowingRepository _borrowingRepository = _fixture.Container.GetInstance<IBorrowingRepository>();

            Borrowing? borrowing = await _borrowingRepository.GetByIdAsync(1);

            Assert.NotNull(borrowing);
            Assert.Equal(1, borrowing.BorrowingId);
            Assert.Equal(1, borrowing.UserId);
            Assert.Equal(BorrowingStatus.Returned, borrowing.Status);
            Assert.Equal(new DateTime(2025, 02, 01), borrowing.DueDate);
        }
    }

    [Fact]
    public async Task GetByIdAsync_WhenIdDoesnotExist_ShouldReturnNull()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBorrowingRepository _borrowingRepository = _fixture.Container.GetInstance<IBorrowingRepository>();

            Borrowing? borrowing = await _borrowingRepository.GetByIdAsync(111);
            Assert.Null(borrowing);
        }
    }

    [Fact]
    public async Task AddAsync_IfRequestIsValid_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBorrowingRepository _borrowingRepository = _fixture.Container.GetInstance<IBorrowingRepository>();
            Borrowing borrowing = new Borrowing
            {
                BookId = 3,
                UserId = 3,
                BorrowDate = new DateTime(2025, 11, 13),
                DueDate = new DateTime(2025, 11, 23),
                Status = BorrowingStatus.Overdue
            };

            Borrowing? addedBorrowing = await _borrowingRepository.AddAsync(borrowing);

            Assert.NotNull(addedBorrowing);
            Assert.Equal(3, addedBorrowing.BorrowingId);
            Assert.Equal(3, addedBorrowing.UserId);
            Assert.Equal(3, addedBorrowing.BookId);
            Assert.Equal(new DateTime(2025, 11, 23), addedBorrowing.DueDate);
        }
    }

    [Fact]
    public async Task AddAsync_IfRequestIsInvalid_ShouldThrowDbUpdateException()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBorrowingRepository _borrowingRepository = _fixture.Container.GetInstance<IBorrowingRepository>();
            LibraryManagementDbContext context = _fixture.Container.GetInstance<LibraryManagementDbContext>();
            Borrowing borrowing = new Borrowing();

            await Assert.ThrowsAsync<DbUpdateException>(async () =>
            {
                await _borrowingRepository.AddAsync(borrowing);
                await context.SaveChangesAsync();
            });
        }
    }

    [Fact]
    public async Task UpdateAsync_IfRequestIsValid_ShouldReturnEntity()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBorrowingRepository _borrowingRepository = _fixture.Container.GetInstance<IBorrowingRepository>();
            LibraryManagementDbContext context = _fixture.Container.GetInstance<LibraryManagementDbContext>();
            Borrowing borrowing = new Borrowing
            {
                BorrowingId = 2,
                BookId = 2,
                UserId = 2,
                BorrowDate = new DateTime(2025, 11, 12),
                DueDate = new DateTime(2025, 11, 22),
                Status = BorrowingStatus.Returned
            };

            await _borrowingRepository.UpdateAsync(borrowing);
            await context.SaveChangesAsync();

            var updatedBorrowing = await _borrowingRepository.GetByIdAsync(2);

            Assert.NotNull(updatedBorrowing);
            Assert.Equal(2, updatedBorrowing.BorrowingId);
            Assert.Equal(2, updatedBorrowing.UserId);
            Assert.Equal(2, updatedBorrowing.BookId);
            Assert.Equal(new DateTime(2025, 11, 22), updatedBorrowing.DueDate);
        }
    }

    [Fact]
    public async Task UpdateAsync_IfRequestIsInvalid_ShouldThrowDbUpdateException()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBorrowingRepository _borrowingRepository = _fixture.Container.GetInstance<IBorrowingRepository>();
            LibraryManagementDbContext context = _fixture.Container.GetInstance<LibraryManagementDbContext>();
            Borrowing borrowing = new Borrowing();

            await Assert.ThrowsAsync<DbUpdateException>(async () =>
            {
                await _borrowingRepository.UpdateAsync(borrowing);
                await context.SaveChangesAsync();
            });
        }
    }

    [Fact]
    public async Task GetAllAsync_WhenIdsExist_ShouldReturnEntities()
    {
        using (AsyncScopedLifestyle.BeginScope(_fixture.Container))
        {
            IBorrowingRepository _borrowingRepository = _fixture.Container.GetInstance<IBorrowingRepository>();

            ICollection<Borrowing> borrowings = await _borrowingRepository.GetAllAsync();

            Assert.NotNull(borrowings);
            Assert.Equal(2, borrowings.Count);
        }
    }
}