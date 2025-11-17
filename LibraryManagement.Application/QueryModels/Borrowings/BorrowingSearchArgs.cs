using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.QueryModels.Borrowings
{
    public class BorrowingSearchArgs
    {
        // is this correct? didn't find any specification for this in the task description.
        public string? SearchTerm { get; set; }
        public bool? IsActive { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
