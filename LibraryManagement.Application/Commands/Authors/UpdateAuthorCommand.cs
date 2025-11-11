namespace LibraryManagement.Application.Commands.Author
{
    public class UpdateAuthorCommand
    {
        public long? Authorid { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Biography { get; set; }
        public string? DateOfBirth { get; set; } // ISO 8601 format?
        public bool? IsActive { get; set; }
    }
}
