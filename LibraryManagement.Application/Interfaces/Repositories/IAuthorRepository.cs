using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Application.QueryModels.Authors;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Interfaces.Repositories
{
    public interface IAuthorRepository
    {
        public Task<Author> GetByIdAsync(long authorId, CancellationToken cancellationToken = default);
        public Task<Author> AddAsync(Author author, CancellationToken cancellationToken = default);
        public Task<Author> UpdateAsync(Author author, CancellationToken cancellationToken = default);
        //public Task<bool> DeleteAsync(Author author, CancellationToken cancellationToken = default);
        public Task<ICollection<Author>> GetAllAsync(CancellationToken cancellationToken = default);
        public Task<Author> FindAsync(long authorId, AuthorSearchArgs args, CancellationToken cancellationToken = default);
    }

}

