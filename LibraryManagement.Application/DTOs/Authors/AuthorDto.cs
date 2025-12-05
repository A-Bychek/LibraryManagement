namespace LibraryManagement.Application.DTOs.Authors;

public class AuthorDto
{
    public long AuthorId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Biography { get; set; } = null!;
    public string DateOfBirth { get; set; } = null!; // ISO 8601 format
    public bool IsActive { get; set; } = true;
    public int BookCount { get; set; }
}
