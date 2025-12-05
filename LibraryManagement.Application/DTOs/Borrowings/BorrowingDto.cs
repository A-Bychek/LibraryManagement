namespace LibraryManagement.Application.DTOs.Borrowings;

public class BorrowingDto
{
    public int BorrowingId { get; set; }
    public long BookId { get; set; }
    public string BookTitle { get; set; } = null!;
    public long UserId { get; set; }
    public string BorrowDate { get; set; } = null!; // ISO 8601 format?
    public string DueDate { get; set; } = null!; // ISO 8601 format?
    public string? ReturnDate { get; set; } // ISO 8601 format?
    public string Status { get; set; } = null!;
    public double? FineAmount { get; set; } // arbitrary, up to me?
}
