using FluentAssertions;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.DTOs.Authors;
using LibraryManagement.Contract.Commands.Author;
using LibraryManagement.Integration.Tests.Base;

namespace LibraryManagement.Integration.Tests.Application.Mappings;

public class AuthorMappingTests : AutoMapperTestBase
{
    [Fact]
    public void Author_To_AuthorDto_ShouldMapCorrectly()
    {
        Author author = new Author
        {
            AuthorId = 1,
            FirstName = "Test First Name",
            LastName = "Test Last Name",
            Biography = "Test Biography",
            DateOfBirth = new DateTime(1970, 01, 01),
            IsActive = true
        };

        var authorDto = _mapper.Map<AuthorDto>(author);

        authorDto.Should().NotBeNull();
        authorDto.AuthorId.Should().Be(author.AuthorId);
        authorDto.FirstName.Should().Be(author.FirstName);
        authorDto.LastName.Should().Be(author.LastName);
        authorDto.Biography.Should().Be(author.Biography);
        authorDto.DateOfBirth.Should().Be(author.DateOfBirth.ToString());
        authorDto.IsActive.Should().Be(author.IsActive);
    }

    [Fact]
    public void CreateAuthorCommand_To_Author_ShouldMapCorrectly()
    {
        CreateAuthorCommand createAuthorCommand = new CreateAuthorCommand
        {
            FirstName = "Test First Name",
            LastName = "Test Last Name",
            Biography = "Test Biography",
            DateOfBirth = "1970-01-01"
        };

        var author = _mapper.Map<Author>(createAuthorCommand);

        author.Should().NotBeNull();
        author.AuthorId.Equals(1);
        author.FirstName.Should().Be(createAuthorCommand.FirstName);
        author.LastName.Should().Be(createAuthorCommand.LastName);
        author.Biography.Should().Be(createAuthorCommand.Biography);
        author.DateOfBirth.Should().Be(DateTime.Parse(createAuthorCommand.DateOfBirth));
        author.IsActive.Equals(true);
    }
}
