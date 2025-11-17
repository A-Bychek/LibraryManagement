using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Domain.Entities
{
    public class User
    {
        public long UserId;    
        public Borrowing Borrowing {  get; set; }
    }
}
