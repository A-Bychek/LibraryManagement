namespace LibraryManagement.Application.Commands.Book
{
    public class UpdateBookCommand
    {
        public long? BookId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public long? CategoryId { get; set; }
        public string? PublishedDate { get; set; } // ISO 8601 format?
        public int? PageCount { get; set; }
    }
}
