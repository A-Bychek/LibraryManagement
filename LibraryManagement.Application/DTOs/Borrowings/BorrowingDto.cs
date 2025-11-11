namespace LibraryManagement.Application.DTOs.Borrowing
{
    public class BorrowingDto
    {
        public int? BorrowingId { get; set; }
        public long? BookId { get; set; }
        public string? BookTitle { get; set; }
        public long? UserId { get; set; }
        public string? BorrowDate { get; set; } // ISO 8601 format?
        public string? DueDate { get; set; } // ISO 8601 format?
        public string? ReturnDate { get; set; } // ISO 8601 format?
        public string? Status { get; set; }
        public double? FineAmount { get; set; } // arbitrary, up to me?
    }
}
