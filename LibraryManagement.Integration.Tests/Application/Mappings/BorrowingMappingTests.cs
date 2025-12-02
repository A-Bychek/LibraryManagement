using FluentAssertions;
using LibraryManagement.Application.DTOs.Borrowings;
using LibraryManagement.Contract.Commands.Borrowing;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Integration.Tests.Base;

namespace LibraryManagement.Integration.Tests.Application.Mappings;

public class BorrowingMappingTests : AutoMapperTestBase
{
    [Fact]
    public void Borrowing_To_BorrowingDto_ShouldMapCorrectly()
    {
        Borrowing borrowing = new Borrowing
        {
            BorrowingId = 1,
            BookId = 1,
            UserId = 1,
            BorrowDate = new DateTime(2025, 11, 30),
            DueDate = new DateTime(2025, 12, 12),
            Status = BorrowingStatus.Active
        };

        var borrowingDto = _mapper.Map<BorrowingDto>(borrowing);

        borrowingDto.Should().NotBeNull();
        borrowingDto.BorrowingId.Should().Be(borrowing.BorrowingId);
        borrowingDto.BookId.Should().Be(borrowing.BookId);
        borrowingDto.UserId.Should().Be(borrowing.UserId);
        borrowingDto.BorrowDate.Should().Be(borrowing.BorrowDate.ToString());
        borrowingDto.DueDate.Should().Be(borrowing.DueDate.ToString());
        borrowingDto.Status.Should().Be(borrowing.Status.ToString());
    }

    [Fact]
    public void BorrowingBookCommand_To_Borrowing_ShouldMapCorrectly()
    {
        BorrowBookCommand borrowBookCommand = new BorrowBookCommand
        {
            BookId = 1,
            UserId = 1,
            DaysToReturn = 10
        };

        var borrowing = _mapper.Map<Borrowing>(borrowBookCommand);

        borrowing.Should().NotBeNull();
        borrowing.BookId.Should().Be(borrowBookCommand.BookId);
        borrowing.UserId.Should().Be(borrowBookCommand.UserId);
        borrowing.BorrowDate.Should().Be(DateTime.Today);
        borrowing.DueDate.Should().Be(DateTime.Today + TimeSpan.FromDays(borrowBookCommand.DaysToReturn));
        borrowing.Status.Should().Be(BorrowingStatus.Active);
    }
}
