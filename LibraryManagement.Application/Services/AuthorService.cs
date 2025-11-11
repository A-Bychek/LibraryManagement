using LibraryManagement.Application.Commands.Author;
using LibraryManagement.Application.DTOs.Author;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Application.QueryModels.Authors;
using LibraryManagement.Application.Interfaces.Repositories;
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
        public IAuthorRepository _context { get; set; } = null!;
        public AuthorService(IAuthorRepository context) 
        {
            _context = context;
        }

        public async Task<List<AuthorDto>> GetAuthorsAsync(AuthorSearchArgs authorSearchArgs, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public async Task<AuthorDto> GetAuthorAsync(long authorId, CancellationToken cancellationToken)
        {
            var author = await _context.GetByIdAsync(authorId, cancellationToken) ?? throw new Exception($"Can't find a {authorId} author"!);
            return author;
        }
        public async Task<AuthorDto> CreateAuthorAsync(CreateAuthorCommand createAuthorCommand, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public async Task<AuthorDto> UpdateAuthorAsync(UpdateAuthorCommand updateAuthorCommand, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public async Task<int> GetAuthorBookCountAsync(long authorId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
