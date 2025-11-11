namespace LibraryManagement.Application.Commands.Author
{
    public class CreateAuthorCommand
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Biography { get; set; }
        public string? DateofBirth { get; set; } // ISO 8601 format?
    }
}
