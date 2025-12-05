using AutoMapper;
using Grpc.Core;
using LibraryManagement.Application.DTOs.Authors;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Contract.Authors;
using LibraryManagement.Contract.Commands.Author;
using LibraryManagement.Contract.QueryModels.Authors;
using Serilog;

namespace LibraryManagement.Api.Services;

public class GrpcAuthorService: AuthorService.AuthorServiceBase
{
    private readonly IAuthorService _authorService;
    private readonly IMapper _mapper;
    private readonly Serilog.ILogger _logger;

    public GrpcAuthorService(IAuthorService authorService, IMapper mapper)
    {
        _authorService = authorService;
        _mapper = mapper;
        _logger = Log.Logger;
    }

    public override async Task<AuthorGetResponse> GetAuthor(AuthorGetRequest request, ServerCallContext context)
    {
        AuthorDto author = await _authorService.GetAuthorAsync(request.AuthorId, context.CancellationToken);
        _logger.Information($"author ID: {author.AuthorId}, Name: {author.FirstName} {author.LastName}, date of birth: {author.DateOfBirth}, book count: {author.BookCount}");

        return new AuthorGetResponse
        {
            Author = _mapper.Map<AuthorResponse>(author)
        };
    }

    public override async Task<AuthorResponse> CreateAuthor(CreateAuthorRequest request, ServerCallContext context)
    {
        var createAuthorCommand = _mapper.Map<CreateAuthorCommand>(request);
        var author = await _authorService.CreateAuthorAsync(createAuthorCommand, context.CancellationToken);
        _logger.Information($"Author entity has been created: ID: {author.AuthorId}," +
            $"Name: {author.FirstName} {author.LastName}, Bio: {author.Biography}, Date of birth: {author.DateOfBirth}," +
            $"isActive: {author.IsActive}");
        return _mapper.Map<AuthorResponse>(author);
    }

    public override async Task<AuthorListResponse> GetAuthors(AuthorSearchRequest request, ServerCallContext context)
    {
        var searchAuthorCommand = _mapper.Map<AuthorSearchArgs>(request);

        var authors = await _authorService.GetAuthorsAsync(searchAuthorCommand, context.CancellationToken);

        var mappedAuthors = authors.Items.Select(_mapper.Map<AuthorResponse>);

        var authorResponse = _mapper.Map<AuthorListResponse>(authors, opt => opt.AfterMap((src, dest) => dest.Authors.AddRange(mappedAuthors)));

        _logger.Information($"{authors.TotalCount} authors have been found.");
        return authorResponse;
    }

    public override async Task<AuthorResponse> UpdateAuthor(UpdateAuthorRequest request, ServerCallContext context)
    {
        var updateAuthorCommand = _mapper.Map<UpdateAuthorCommand>(request);
        var author = await _authorService.UpdateAuthorAsync(updateAuthorCommand, context.CancellationToken);
        _logger.Information($"Author entity has been updated: ID: {author.AuthorId}," +
            $"Name: {author.FirstName} {author.LastName}, Bio: {author.Biography}, Date of birth: {author.DateOfBirth}, " +
            $"isActive: {author.IsActive}");
        return _mapper.Map<AuthorResponse>(author);
    }
}
