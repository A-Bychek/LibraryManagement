using AutoMapper;
using Grpc.Core;
using LibraryManagement.Application.Commands.Author;
using LibraryManagement.Application.DTOs.Authors;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Application.QueryModels.Authors;
using LibraryManagement.Contract.Authors;
using LibraryManagement.Shared.Exceptions;
using Serilog;
using System.ComponentModel.DataAnnotations;


namespace LibraryManagement.Api.Services
{
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
            try
            {
                AuthorDto author = await _authorService.GetAuthorAsync(request.AuthorId, context.CancellationToken);
                _logger.Information($"author ID: {author.AuthorId}, Name: {author.FirstName} {author.LastName}, date of birth: {author.DateOfBirth}, book count: {author.BookCount}");

                return new AuthorGetResponse
                {
                    Author = _mapper.Map<AuthorResponse>(author)
                };
            }

            catch (NotFoundException exc)
            {
                _logger.Error($"Not found: {exc.Message}");
                throw new RpcException(new Status(StatusCode.NotFound, exc.Message));
            }
            catch (Exception exc)
            {
                _logger.Error($"Unknown issue occured: {exc.Message}");
                throw new RpcException(new Status(StatusCode.Unknown, $"Unknown issue: Message => {exc.Message}," +
                    $"Source => {exc.Source}, Data => {exc.Data}"));
            }
        }

        public override async Task<AuthorResponse> CreateAuthor(CreateAuthorRequest request, ServerCallContext context)
        {
            try
            {
                var createAuthorCommand = _mapper.Map<CreateAuthorCommand>(request);
                var author = await _authorService.CreateAuthorAsync(createAuthorCommand, context.CancellationToken);
                _logger.Information($"Author entity has been created: ID: {author.AuthorId}," +
                    $"Name: {author.FirstName} {author.LastName}, Bio: {author.Biography}, Date of birth: {author.DateOfBirth}," +
                    $"isActive: {author.IsActive}");
                return _mapper.Map<AuthorResponse>(author);
            }
            catch (ValidationException exc)
            {
                _logger.Error($"Validation failed: {exc.Message}");
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Validation failed."));
            }
            catch (Exception exc)
            {
                _logger.Error($"Unknown issue: {exc.Message}, {exc.InnerException}, {exc.GetBaseException}");
                throw new RpcException(new Status(StatusCode.Unknown, "Unknown issue."));
            }
        }

        public override async Task<AuthorListResponse> GetAuthors(AuthorSearchRequest request, ServerCallContext context)
        {
            try
            {
                var searchAuthorCommand = _mapper.Map<AuthorSearchArgs>(request);

                var authors = await _authorService.GetAuthorsAsync(searchAuthorCommand, context.CancellationToken);

                var mappedAuthors = authors.Items.Select(_mapper.Map<AuthorResponse>);

                var authorResponse = new AuthorListResponse
                {
                    TotalCount = authors.TotalCount,
                    PageNumber = authors.PageNumber,
                    PageSize = authors.PageSize
                };

                authorResponse.Authors.AddRange(mappedAuthors);
                _logger.Information($"{authors.TotalCount} authors have been found.");
                return authorResponse;
            }
            catch (ValidationException exc)
            {
                _logger.Error($"Validation failed: {exc.Message}");
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Validation failed."));
            }
            catch (Exception exc)
            {
                _logger.Error($"Unknown issue: {exc.Message}");
                throw new RpcException(new Status(StatusCode.Unknown, "Unknown issue."));
            }
        }

        public override async Task<AuthorResponse> UpdateAuthor(UpdateAuthorRequest request, ServerCallContext context)
        {
            try
            {
                var updateAuthorCommand = _mapper.Map<UpdateAuthorCommand>(request);
                var author = await _authorService.UpdateAuthorAsync(updateAuthorCommand, context.CancellationToken);
                _logger.Information($"Author entity has been updated: ID: {author.AuthorId}," +
                    $"Name: {author.FirstName} {author.LastName}, Bio: {author.Biography}, Date of birth: {author.DateOfBirth}, " +
                    $"isActive: {author.IsActive}");
                return _mapper.Map<AuthorResponse>(author);
            }
            catch (ValidationException exc)
            {
                _logger.Error($"Validation failed: {exc.Message}");
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Validation failed."));
            }
            catch (Exception exc)
            {
                _logger.Error($"Unknown issue: {exc.Message}");
                throw new RpcException(new Status(StatusCode.Unknown, "Unknown issue."));
            }
        }
    }
}
