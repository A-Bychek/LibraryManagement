namespace LibraryManagement.Application.DTOs.Categories
{
    public class CategoryDto
    {
        public long? CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public long? ParentCategoryId { get; set; }
        public string? ParentCategoryName { get; set; }
        public int? SortOrder { get; set; }
        public bool? IsActive { get; set; }
        public int BookCount { get; set; }
        public ICollection<CategoryDto>? SubCategories { get; set; }
    }
}
