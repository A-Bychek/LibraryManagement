namespace LibraryManagement.Application.QueryModels.Categories
{
    public class CategorySearchArgs
    {
        public string? SearchTerm { get; set; }
        public long? ParentCategoryId { get; set; }
        public bool? IsActive { get; set; }
    }
}
