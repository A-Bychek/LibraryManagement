using AutoMapper;
using FluentValidation;
using LibraryManagement.Application.Commands.Author;
using LibraryManagement.Application.DTOs.Authors;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Application.QueryModels.Authors;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Shared;
using LibraryManagement.Shared.Exceptions;

namespace LibraryManagement.Application.Services.Authors
{
    public class AuthorService : IAuthorService
    {
        private IAuthorRepository _authorRepository { get; set; } = null!;
        private IMapper _mapper { get; set; } = null!;
        private IValidator<CreateAuthorCommand> _createAuthorCommandValidator { get; set; }
        private IValidator<UpdateAuthorCommand> _updateAuthorCommandValidator { get; set; }
        public AuthorService(
            IAuthorRepository authorRepository, 
            IMapper mapper,
            IValidator<CreateAuthorCommand> createAuthorCommandValidator,
            IValidator<UpdateAuthorCommand> updateAuthorCommandValidator
            )
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _createAuthorCommandValidator = createAuthorCommandValidator;
            _updateAuthorCommandValidator = updateAuthorCommandValidator;
        }

        public async Task<PagedResult<AuthorDto>> GetAuthorsAsync(AuthorSearchArgs authorSearchArgs, CancellationToken cancellationToken)
        {
            var authors = await _authorRepository.FindAsync(authorSearchArgs, cancellationToken);

            List<AuthorDto> mappedAuthors = new List<AuthorDto>();
            foreach (var author in authors)
            {
                AuthorDto mappedAuthor = _mapper.Map<AuthorDto>(author);
                mappedAuthors.Append(mappedAuthor);
            }

            return PagedResult<AuthorDto>.Create(mappedAuthors, authors.TotalCount, authors.PageNumber, authors.PageSize); // re-check this
        }
        public async Task<AuthorDto> GetAuthorAsync(long authorId, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetByIdAsync(authorId, cancellationToken) ?? throw new NotFoundException($"Can't find a {authorId} author"!);
            return _mapper.Map<AuthorDto>(author);
        }
        public async Task<AuthorDto> CreateAuthorAsync(CreateAuthorCommand createAuthorCommand, CancellationToken cancellationToken)
        {
            await _createAuthorCommandValidator.ValidateAndThrowAsync(createAuthorCommand, cancellationToken);
            var author = new Author()
            {

                FirstName = createAuthorCommand.FirstName,
                LastName = createAuthorCommand.LastName,
                Biography = createAuthorCommand.Biography,
                DateOfBirth = Convert.ToDateTime(createAuthorCommand.DateOfBirth)
            };
            await _authorRepository.AddAsync(author, cancellationToken);
            return _mapper.Map<AuthorDto>(author);
        }
        public async Task<AuthorDto> UpdateAuthorAsync(UpdateAuthorCommand updateAuthorCommand, CancellationToken cancellationToken)
        {
            await _updateAuthorCommandValidator.ValidateAndThrowAsync (updateAuthorCommand, cancellationToken);
            var author = await _authorRepository.GetByIdAsync(updateAuthorCommand.AuthorId, cancellationToken) ?? throw new NotFoundException($"Can't find a {updateAuthorCommand.AuthorId} author!");
            author.FirstName = updateAuthorCommand.FirstName ?? author.FirstName;
            author.LastName = updateAuthorCommand.LastName ?? author.LastName;
            author.DateOfBirth = updateAuthorCommand.DateOfBirth is not null ? Convert.ToDateTime(updateAuthorCommand.DateOfBirth) : author.DateOfBirth;
            author.Biography = updateAuthorCommand.Biography ?? author.Biography;
            author.IsActive = updateAuthorCommand.IsActive ?? author.IsActive;

            await _authorRepository.UpdateAsync(author, cancellationToken);
            return _mapper.Map<AuthorDto>(author);
        }
        public async Task<int> GetAuthorBookCountAsync(long authorId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            // return await _context.Books.CountAsync(x => x.AuthorId = authorId); // (re-check the context)
        }

        public async Task<AuthorDto> DeleteAuthorAsync(long authorId, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetByIdAsync(authorId, cancellationToken);
            await _authorRepository.DeleteAsync(author, cancellationToken);
            return _mapper.Map<AuthorDto>(author); // change, return the operation status
        }
    }
}
