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
        [Key]
        public long CategoryId { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Category ParentCategory { get; set; } = null!;
        public long? ParentCategoryId { get; set; }
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();
        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
