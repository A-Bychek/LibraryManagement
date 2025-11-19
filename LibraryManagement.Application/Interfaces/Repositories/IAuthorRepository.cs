using LibraryManagement.Application.QueryModels.Authors;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Shared;

namespace LibraryManagement.Application.Interfaces.Repositories
{
    public interface IAuthorRepository
    {
        public Task<Author?> GetByIdAsync(long authorId, CancellationToken cancellationToken = default);
        public Task<Author> AddAsync(Author author, CancellationToken cancellationToken = default);
        public Task<Author> UpdateAsync(Author author, CancellationToken cancellationToken = default);
        public Task<Author> DeleteAsync(Author author, CancellationToken cancellationToken = default);
        public Task<ICollection<Author>> GetAllAsync(CancellationToken cancellationToken = default);
        public Task<PagedResult<Author>> FindAsync(AuthorSearchArgs authorSearchArgs, CancellationToken cancellationToken = default);
    }

}

