using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.QueryModels.Books
{
    public class BookSearchArgs
    {
        public string? SearchTerm { get; set; }
        public long? AuthorId { get; set; }
        public long? CategoryId { get; set; }
        public bool? IsAvailable { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
