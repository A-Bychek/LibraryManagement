using System;

namespace LibraryManagement.Application.Commands.Borrowing
{
    public class BorrowBookCommand
    {
        public long BookId { get; set; }
        public long UserId { get; set; }
        public int DaysToReturn { get; set; } = 14;
    }
}
