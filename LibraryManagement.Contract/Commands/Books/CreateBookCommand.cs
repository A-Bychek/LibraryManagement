namespace LibraryManagement.Contract.Commands.Book;

public  class CreateBookCommand
{
    public string Title { get; set; } = null!;
    public string ISBN { get; set; } = null!;
    public string Description { get; set; } = null!;
    public long AuthorId { get; set; }
    public long CategoryId { get; set; }
    public string PublishedDate { get; set; } = null!; // ISO 8601 format?
    public int PageCount { get; set; }
}
