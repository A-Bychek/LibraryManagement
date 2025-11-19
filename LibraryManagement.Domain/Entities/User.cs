namespace LibraryManagement.Domain.Entities
{
    public class User
    {
        public long UserId;
        public Borrowing Borrowing { get; set; } = null!;
    }
}
