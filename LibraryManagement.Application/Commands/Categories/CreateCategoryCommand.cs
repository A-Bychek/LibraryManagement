namespace LibraryManagement.Application.Commands.Category
{
    public class CreateCategoryCommand
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public long? ParentCategoryId { get; set; }
        public int? SortOrder { get; set; }
    }
}
