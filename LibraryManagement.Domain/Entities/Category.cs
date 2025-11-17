using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Domain.Entities
{
    public class Category
    {
        public Category(string name, string? description, long? categoryId, int sortOrder, bool isActive)
        {
            Name = name;
            Description = description;
            ParentCategoryId = ParentCategoryId;
            SortOrder = sortOrder;
            IsActive = isActive;
        }

        public long CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Category ParentCategory { get; set; } = null!;
        public long? ParentCategoryId { get; set; }
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
