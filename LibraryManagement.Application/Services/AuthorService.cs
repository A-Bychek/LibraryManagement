using LibraryManagement.Application.Commands.Author;
using LibraryManagement.Application.DTOs.Author;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Application.QueryModels.Authors;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Shared;
using LibraryManagement.Application.Mappings;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private IAuthorRepository _context { get; set; } = null!;
        private IMapper _mapper { get; set; } = null!;
        public AuthorService(IAuthorRepository context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<AuthorDto>> GetAuthorsAsync(AuthorSearchArgs authorSearchArgs, CancellationToken cancellationToken)
        {
            var authors = await _context.FindAsync(authorSearchArgs, cancellationToken);

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
            var author = await _context.GetByIdAsync(authorId, cancellationToken) ?? throw new Exception($"Can't find a {authorId} author"!);
            return _mapper.Map<AuthorDto>(author);
        }
        public async Task<AuthorDto> CreateAuthorAsync(CreateAuthorCommand createAuthorCommand, CancellationToken cancellationToken)
        {
            var author = new Author()
            {

                FirstName = createAuthorCommand.FirstName,
                LastName = createAuthorCommand.LastName,
                Biography = createAuthorCommand.Biography,
                DateOfBirth = Convert.ToDateTime(createAuthorCommand.DateOfBirth)
            };
            await _context.AddAsync(author, cancellationToken);
            return _mapper.Map<AuthorDto>(author);
        }
        public async Task<AuthorDto> UpdateAuthorAsync(UpdateAuthorCommand updateAuthorCommand, CancellationToken cancellationToken)
        {
            var author = await _context.GetByIdAsync(updateAuthorCommand.AuthorId, cancellationToken) ?? throw new Exception($"Can't find a {updateAuthorCommand.AuthorId} author!");
            author.FirstName = updateAuthorCommand.FirstName ?? author.FirstName;
            author.LastName = updateAuthorCommand.LastName ?? author.LastName;
            author.DateOfBirth = updateAuthorCommand.DateOfBirth is not null ? Convert.ToDateTime(updateAuthorCommand.DateOfBirth) : author.DateOfBirth;
            author.Biography = updateAuthorCommand.Biography ?? author.Biography;
            author.IsActive = updateAuthorCommand.IsActive ?? author.IsActive;

            await _context.UpdateAsync(author, cancellationToken);
            return _mapper.Map<AuthorDto>(author);
        }
        public async Task<int> GetAuthorBookCountAsync(long authorId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            // return await _context.Books.CountAsync(x => x.AuthorId = authorId); // (re-check the context)
        }

        public async Task<AuthorDto> DeleteAuthorAsync(long authorId, CancellationToken cancellationToken)
        {
            var author = await _context.GetByIdAsync(authorId, cancellationToken);
            await _context.DeleteAsync(author, cancellationToken);
            return _mapper.Map<AuthorDto>(author);
        }
    }
}
