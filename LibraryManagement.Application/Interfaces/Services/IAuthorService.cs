using LibraryManagement.Application.Commands.Author;
using LibraryManagement.Application.DTOs.Author;
using LibraryManagement.Application.QueryModels.Authors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Interfaces.Services
{
    public interface IAuthorService
    {
        public Task<AuthorDto> GetAuthorAsync(long authorId, CancellationToken cancellationToken);
        public Task<List<AuthorDto>> GetAuthorsAsync(AuthorSearchArgs args, CancellationToken cancellationToken);
        public Task<AuthorDto> CreateAuthorAsync(CreateAuthorCommand command, CancellationToken cancellationToken);
        public Task<AuthorDto> UpdateAuthorAsync(UpdateAuthorCommand command, CancellationToken cancellationToken);
        public Task<int> GetAuthorBookCountAsync(long authorId, CancellationToken cancellationToken);
    }
}