using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Domain.Entities
{
    public class Borrowing
    {
        public Borrowing(long bookId, long userId, DateTime borrowDate, DateTime dueDate, DateTime? returnDate, BorrowingStatus status)
        {
            BookId = bookId;
            UserId = userId;
            BorrowDate = borrowDate;
            DueDate = dueDate;
            ReturnDate = returnDate;
            Status = status;
        }
        private Borrowing() { }
        public int BorrowingId { get; set; }
        public long BookId { get; set; }
        public Book Book { get; set; } = null!;
        public User User { get; set; } = null!;
        public long UserId { get; set; }
        public DateTime BorrowDate {  get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public BorrowingStatus Status { get; set; }
    }
}
