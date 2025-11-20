namespace LibraryManagement.Application.Commands.Author
{
    public class CreateAuthorCommand
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Biography { get; set; } = null!;
        public string DateOfBirth { get; set; } = null!; // ISO 8601 format?
    }
}
