using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.QueryModels.Authors;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class AuthorRepository: IAuthorRepository
    {
        public Task<Author> GetByIdAsync(long authorId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<Author> AddAsync(Author author, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<Author> UpdateAsync(Author author, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(Author author, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<ICollection<Author>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<Author> FindAsync(long authorId, AuthorSearchArgs args, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
