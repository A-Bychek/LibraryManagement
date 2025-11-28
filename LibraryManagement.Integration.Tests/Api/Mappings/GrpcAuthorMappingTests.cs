using FluentAssertions;
using LibraryManagement.Application.DTOs.Authors;
using LibraryManagement.Contract.Authors;
using LibraryManagement.Contract.Commands.Author;
using LibraryManagement.Contract.QueryModels.Authors;
using LibraryManagement.Integration.Tests.Base;
using LibraryManagement.Shared;

namespace LibraryManagement.Integration.Tests.Api.Mappings;

public class GrpcAuthorMappingTests : AutoMapperTestBase
{
    [Fact]
    public void AuthorDto_To_AuthorResponse_ShouldMapCorrectly()
    {
        AuthorDto authorDto = new AuthorDto
        {
            AuthorId = 1,
            FirstName = "Test First Name",
            LastName = "Test Last Name",
            Biography = "Test Biography",
            DateOfBirth = "1980-01-01",
            IsActive = true,
            BookCount = 1
        };

        var authorResponse = _mapper.Map<AuthorResponse>(authorDto);

        authorResponse.Should().NotBeNull();
        authorResponse.AuthorId.Should().Be(authorDto.AuthorId);
        authorResponse.FirstName.Should().Be(authorDto.FirstName);
        authorResponse.LastName.Should().Be(authorDto.LastName);
        authorResponse.Biography.Should().Be(authorDto.Biography);
        authorResponse.DateOfBirth.Should().Be(authorDto.DateOfBirth);
        authorResponse.IsActive.Should().Be(authorDto.IsActive);
        authorResponse.BookCount.Should().Be(authorDto.BookCount);
    }

    [Fact]
    public void CreateAuthorRequest_To_CreateAuthorCommand_ShouldMapCorrectly()
    {
        CreateAuthorRequest createAuthorRequest = new CreateAuthorRequest
        {
            FirstName = "Test First Name",
            LastName = "Test Last Name",
            Biography = "Test Biography",
            DateOfBirth = "1980-01-01"
        };

        var createAuthorCommand = _mapper.Map<CreateAuthorCommand>(createAuthorRequest);

        createAuthorCommand.Should().NotBeNull();
        createAuthorCommand.FirstName.Should().Be(createAuthorRequest.FirstName);
        createAuthorCommand.LastName.Should().Be(createAuthorRequest.LastName);
        createAuthorCommand.Biography.Should().Be(createAuthorRequest.Biography);
        createAuthorCommand.DateOfBirth.Should().Be(createAuthorRequest.DateOfBirth);
    }

    [Fact]
    public void UpdateAuthorRequest_To_UpdateAuthorCommand_ShouldMapCorrectly()
    {
        UpdateAuthorRequest updateAuthorRequest = new UpdateAuthorRequest
        {
            AuthorId = 1,
            FirstName = "Test First Name",
            LastName = "Test Last Name",
            Biography = "Test Biography",
            DateOfBirth = "1980-01-01",
            IsActive = true
        };

        var updateAuthorCommand = _mapper.Map<UpdateAuthorCommand>(updateAuthorRequest);

        updateAuthorCommand.Should().NotBeNull();
        updateAuthorCommand.AuthorId.Should().Be(updateAuthorRequest.AuthorId);
        updateAuthorCommand.FirstName.Should().Be(updateAuthorRequest.FirstName);
        updateAuthorCommand.LastName.Should().Be(updateAuthorRequest.LastName);
        updateAuthorCommand.Biography.Should().Be(updateAuthorRequest.Biography);
        updateAuthorCommand.DateOfBirth.Should().Be(updateAuthorRequest.DateOfBirth);
    }

    [Fact]
    public void AuthorSearchRequest_To_AuthorSearchArgs_ShouldMapCorrectly()
    {
        AuthorSearchRequest authorSearchRequest = new AuthorSearchRequest
        {
            SearchTerm = "TestTerm",
            IsActive = true,
            PageSize = 15
        };

        var authorSearchArgs = _mapper.Map<AuthorSearchArgs>(authorSearchRequest);

        authorSearchArgs.Should().NotBeNull();
        authorSearchArgs.SearchTerm.Should().Be(authorSearchRequest.SearchTerm);
        authorSearchArgs.IsActive.Should().Be(authorSearchRequest.IsActive);
        authorSearchArgs.PageNumber.Equals(1);
        authorSearchArgs.PageSize.Should().Be(authorSearchRequest.PageSize);
    }

    [Fact]
    public void PagedResultAuthorDto_To_AuthorListResponse_ShouldMapCorrectly()
    {
        AuthorDto authorDto = new AuthorDto
        {
            AuthorId = 1,
            FirstName = "Test First Name",
            LastName = "Test Last Name",
            Biography = "Test Biography",
            DateOfBirth = "1980-01-01",
            IsActive = true,
            BookCount = 1
        };

        PagedResult<AuthorDto> pagedResultAuthorDto = PagedResult<AuthorDto>.Create([authorDto], 1, 1, 15);

        var mappedAuthors = pagedResultAuthorDto.Items.Select(_mapper.Map<AuthorResponse>);

        var authorListResponse = _mapper.Map<AuthorListResponse>(pagedResultAuthorDto, 
            opt => opt.AfterMap((src, dest) => dest.Authors.AddRange(mappedAuthors)));

        authorListResponse.Should().NotBeNull();
        authorListResponse.Authors.Should().HaveCount(1);
        authorListResponse.Authors[0].AuthorId.Should().Be(authorDto.AuthorId);
        authorListResponse.Authors[0].FirstName.Should().Be(authorDto.FirstName);
        authorListResponse.Authors[0].LastName.Should().Be(authorDto.LastName);
        authorListResponse.Authors[0].Biography.Should().Be(authorDto.Biography);
        authorListResponse.Authors[0].DateOfBirth.Should().Be(authorDto.DateOfBirth);
        authorListResponse.Authors[0].BookCount.Should().Be(authorDto.BookCount);
        authorListResponse.TotalCount.Should().Be(pagedResultAuthorDto.TotalCount);
        authorListResponse.PageNumber.Should().Be(pagedResultAuthorDto.PageNumber);
        authorListResponse.PageSize.Should().Be(pagedResultAuthorDto.PageSize);
    }
}