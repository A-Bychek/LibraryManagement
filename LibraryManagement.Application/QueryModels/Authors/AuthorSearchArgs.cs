namespace LibraryManagement.Application.QueryModels.Authors
{
    public class AuthorSearchArgs
    {
        public string? SearchTerm { get; set; }
        public bool? IsActive { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
