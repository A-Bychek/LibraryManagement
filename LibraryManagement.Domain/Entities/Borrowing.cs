using System.ComponentModel.DataAnnotations;
using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Domain.Entities
{
    public class Borrowing
    {
        [Key]
        public int BorrowingId { get; set; }
        public long BookId { get; set; }
        public Book Book { get; set; } = null!;
        public long UserId { get; set; }
        [Required]
        public DateTime BorrowDate {  get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        public DateTime? ReturnDate { get; set; }
        public BorrowingStatus Status { get; set; }
    }
}
