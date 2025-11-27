using AutoMapper;
using FluentValidation;
using LibraryManagement.Application.DTOs.Authors;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Contract.Commands.Author;
using LibraryManagement.Contract.QueryModels.Authors;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Shared;
using LibraryManagement.Shared.Exceptions;

namespace LibraryManagement.Application.Services.Authors;

public class AuthorService : IAuthorService
{
    private IAuthorRepository _authorRepository { get; set; } = null!;
    private IMapper _mapper { get; set; } = null!;
    private IValidator<CreateAuthorCommand> _createAuthorCommandValidator { get; set; }
    private IValidator<UpdateAuthorCommand> _updateAuthorCommandValidator { get; set; }
    private IValidator<AuthorSearchArgs> _authorSearchArgsValidator { get; set; }
    public AuthorService(
        IAuthorRepository authorRepository, 
        IMapper mapper,
        IValidator<CreateAuthorCommand> createAuthorCommandValidator,
        IValidator<UpdateAuthorCommand> updateAuthorCommandValidator,
        IValidator<AuthorSearchArgs> authorSearchArgsValidator
        )
    {
        _authorRepository = authorRepository;
        _mapper = mapper;
        _createAuthorCommandValidator = createAuthorCommandValidator;
        _updateAuthorCommandValidator = updateAuthorCommandValidator;
        _authorSearchArgsValidator = authorSearchArgsValidator;
    }

    public async Task<PagedResult<AuthorDto>> GetAuthorsAsync(AuthorSearchArgs authorSearchArgs, CancellationToken cancellationToken = default)
    {
        await _authorSearchArgsValidator.ValidateAndThrowAsync(authorSearchArgs, cancellationToken);

        var authors = await _authorRepository.FindAsync(authorSearchArgs, cancellationToken);

        var mappedAuthors = authors.Items.Select(_mapper.Map<AuthorDto>);

        return PagedResult<AuthorDto>.Create(mappedAuthors, authors.TotalCount, authors.PageNumber, authors.PageSize); // re-check this
    }

    public async Task<AuthorDto> GetAuthorAsync(long authorId, CancellationToken cancellationToken = default)
    {
        var author = await _authorRepository.GetByIdAsync(authorId, cancellationToken) ?? throw new NotFoundException($"Can't find a {authorId} author!");
        return _mapper.Map<AuthorDto>(author);
    }

    public async Task<AuthorDto> CreateAuthorAsync(CreateAuthorCommand createAuthorCommand, CancellationToken cancellationToken = default)
    {
        await _createAuthorCommandValidator.ValidateAndThrowAsync(createAuthorCommand, cancellationToken);
        var author = new Author()
        {
            FirstName = createAuthorCommand.FirstName,
            LastName = createAuthorCommand.LastName,
            Biography = createAuthorCommand.Biography,
            DateOfBirth = DateTime.Parse(createAuthorCommand.DateOfBirth) // to mapper
        };
        await _authorRepository.AddAsync(author, cancellationToken);
        return _mapper.Map<AuthorDto>(author);
    }

    public async Task<AuthorDto> UpdateAuthorAsync(UpdateAuthorCommand updateAuthorCommand, CancellationToken cancellationToken = default)
    {
        await _updateAuthorCommandValidator.ValidateAndThrowAsync (updateAuthorCommand, cancellationToken);
        var author = await _authorRepository.GetByIdAsync(updateAuthorCommand.AuthorId, cancellationToken) ?? throw new NotFoundException($"Can't find a {updateAuthorCommand.AuthorId} author!");
        author.FirstName = !string.IsNullOrEmpty(updateAuthorCommand.FirstName) ? updateAuthorCommand.FirstName : author.FirstName;
        author.LastName = !string.IsNullOrEmpty(updateAuthorCommand.LastName) ? updateAuthorCommand.LastName: author.LastName;
        author.DateOfBirth = !string.IsNullOrEmpty(updateAuthorCommand.DateOfBirth) ? DateTime.Parse(updateAuthorCommand.DateOfBirth) : author.DateOfBirth;
        author.Biography = !string.IsNullOrEmpty(updateAuthorCommand.Biography) ? updateAuthorCommand.Biography : author.Biography;
        author.IsActive = updateAuthorCommand.IsActive ?? author.IsActive;

        await _authorRepository.UpdateAsync(author, cancellationToken);
        return _mapper.Map<AuthorDto>(author);
    }

    public async Task<int> GetAuthorBookCountAsync(long authorId, CancellationToken cancellationToken = default)
    {
        var author = await GetAuthorAsync(authorId, cancellationToken);
        return author.BookCount;
    }

    public async Task<DeleteAuthorDto> DeleteAuthorAsync(long authorId, CancellationToken cancellationToken = default)
    {
        Author? author = await _authorRepository.GetByIdAsync(authorId, cancellationToken) 
            ?? throw new NotFoundException($"Can't find a {authorId} author!"); 
        await _authorRepository.DeleteAsync(author, cancellationToken);
        return new DeleteAuthorDto
        {
            Success = true,
            Message = $"Successfully removed the author with {authorId} authorId."
        };
    }
}
