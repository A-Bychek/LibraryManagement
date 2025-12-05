namespace LibraryManagement.Application.DTOs.Books;

public class BookDto
{
    public long BookId { get; set; }
    public string Title { get; set; } = null!;
    public string ISBN { get; set; } = null!;
    public string? Description { get; set; }
    public long AuthorId { get; set; } 
    public string AuthorName { get; set; } = null!; // FirstName + LastName?
    public long CategoryId { get; set; }
    public string CategoryName { get; set; } = null!; // Category?
    public string PublishedDate { get; set; } = null!; // ISO 8601 format?
    public int? PageCount { get; set; }
    public bool IsAvailable { get; set; }
}
