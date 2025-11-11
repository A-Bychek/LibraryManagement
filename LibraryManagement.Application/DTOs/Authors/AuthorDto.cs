using System;

namespace LibraryManagement.Application.DTOs.Author
{
    public class AuthorDto
    {
        public long? Authorid { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Biography { get; set; }
        public string? DateOfBirth { get; set; } // ISO 8601 format
        public bool? IsActive { get; set; } = true;
        public int? BookCount { get; set; }
    }
}