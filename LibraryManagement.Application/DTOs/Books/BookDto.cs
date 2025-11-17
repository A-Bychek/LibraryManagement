namespace LibraryManagement.Application.DTOs.Books
{
    public class BookDto
    {
        public long? BookId { get; set; }
        public string? Title { get; set; }
        public string? ISBN { get; set; }
        public string? Description { get; set; }
        public long? AuthorId { get; set; }  
        public string? AuthorName { get; set; } // FirstName + LastName?
        public long? CategoryId { get; set; }
        public string? CategoryName { get; set; } // Category + Subcategory?
        public string? PublishedDate { get; set; } // ISO 8601 format?
        public int? PageCount { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
