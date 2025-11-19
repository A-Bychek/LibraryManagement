using LibraryManagement.Application.Commands.Author;
using LibraryManagement.Application.DTOs.Authors;
using LibraryManagement.Application.QueryModels.Authors;
using LibraryManagement.Shared;

namespace LibraryManagement.Application.Interfaces.Services
{
    public interface IAuthorService
    {
        public Task<AuthorDto> GetAuthorAsync(long authorId, CancellationToken cancellationToken);
        public Task<PagedResult<AuthorDto>> GetAuthorsAsync(AuthorSearchArgs args, CancellationToken cancellationToken);
        public Task<AuthorDto> CreateAuthorAsync(CreateAuthorCommand command, CancellationToken cancellationToken);
        public Task<AuthorDto> UpdateAuthorAsync(UpdateAuthorCommand command, CancellationToken cancellationToken);
        public Task<int> GetAuthorBookCountAsync(long authorId, CancellationToken cancellationToken);
    }
}