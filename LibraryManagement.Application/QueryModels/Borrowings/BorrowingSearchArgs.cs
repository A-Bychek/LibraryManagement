using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.QueryModels.Borrowings;

public class BorrowingSearchArgs
{
    public long UserId { get; set; }
    public BorrowingStatus Status { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
