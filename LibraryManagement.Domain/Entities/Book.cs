namespace LibraryManagement.Domain.Entities;

public class Book
{
    public Book()
    { }
    public Book(string title, string isbn, string description,long authorId, long categoryId, DateTime publishedDate, int pageCount, bool isAvailable, DateTime createdDate, DateTime? updatedDate) 
    {
        Title = title;
        ISBN = isbn;
        Description = description;
        AuthorId = authorId;
        CategoryId = categoryId;
        PublishedDate = publishedDate;
        PageCount = pageCount;
        IsAvailable = isAvailable;
        CreatedDate = createdDate;
        UpdatedDate = updatedDate;
    }
    public long BookId { get; set; }
    public string Title { get; set; } = null!;
    public string ISBN { get; set; } = null!;
    public string? Description { get; set; }
    public long AuthorId { get; set; }
    public Author Author { get; set; } = null!;
    public long CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public DateTime? PublishedDate { get; set; }
    public int? PageCount { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
